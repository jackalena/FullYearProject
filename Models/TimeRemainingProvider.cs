using System;
using System.Diagnostics;
using System.Threading;

namespace FullYearProject.Models;

/// <summary>
///     Class providing time remaining for the quiz using a timer counting down from a specified start time.
/// </summary>
public class TimeRemainingProvider
{
    // Interval between invocations of the TimeRemainingChanged event.
    private static readonly TimeSpan _interval = TimeSpan.FromSeconds(1);

    private readonly Stopwatch _stopwatch = new();
    private Thread? _timerThread;
    private volatile bool _timerThreadRunning;

    /// <summary>
    ///     Whether the timer is currently running.
    /// </summary>
    public bool IsRunning => _timerThreadRunning;

    /// <summary>
    ///     The time at which the timer should start counting down.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public TimeSpan StartTime {
        get;
        set {
            if (_timerThreadRunning)
            {
                throw new InvalidOperationException("Cannot change StartTime while the timer is running.");
            }

            field = value;
        }
    }

    /// <summary>
    ///     The time remaining until the timer reaches zero.
    /// </summary>
    public TimeSpan TimeRemaining { get; private set; }

    /// <summary>
    ///     Creates a new instance of the <see cref="TimeRemainingProvider" /> class, using <see cref="TimeSpan.Zero" />
    ///     as the start time.
    /// </summary>
    public TimeRemainingProvider()
    {
        StartTime = TimeSpan.Zero;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeRemainingProvider" /> class, using the specified start time.
    /// </summary>
    /// <param name="startTime">The time to count down from.</param>
    public TimeRemainingProvider(TimeSpan startTime)
    {
        StartTime = startTime;
    }

    /// <summary>
    ///     Event raised when the time remaining changes.
    /// </summary>
    public event TimeRemainingChangedEventHandler? TimeRemainingChanged;

    /// <summary>
    ///     Event raised when the time remaining reaches zero.
    /// </summary>
    public event Action? TimeRemainingElapsed;

    /// <summary>
    ///     Starts the timer.
    /// </summary>
    public void Start()
    {
        if (_timerThreadRunning)
        {
            throw new InvalidOperationException("Timer is already running.");
        }

        TimeRemaining = StartTime;

        _stopwatch.Restart();
        _timerThreadRunning = true;

        _timerThread = new(TimerThreadTask) { IsBackground = true };
        _timerThread.Start();
    }

    /// <summary>
    ///     Stops the timer.
    /// </summary>
    public void Stop()
    {
        if (!_timerThreadRunning)
        {
            throw new InvalidOperationException("Timer is not running.");
        }

        _timerThreadRunning = false;
        _stopwatch.Stop();
    }

    /// <summary>
    ///     Function that runs in the timer thread to update the time remaining.
    /// </summary>
    private void TimerThreadTask()
    {
        var lastElapsed = -_interval;

        while (_timerThreadRunning)
        {
            var elapsed = _stopwatch.Elapsed;


            if (elapsed < StartTime)
            {
                // If the time remaining is less than the start time, round it down to the nearest second and invoke the
                // TimeRemainingChanged event.
                var roundedTime =
                    new TimeSpan((long) Math.Round((double) (StartTime - elapsed).Ticks / TimeSpan.TicksPerSecond) *
                                 TimeSpan.TicksPerSecond);

                TimeRemainingChanged?.Invoke(this, new(roundedTime));
            }
            else
            {
                // If the time remaining is greater than or equal to the start time, the timer has reached zero and 
                // and invoke the TimeRemainingElapsed event.
                TimeRemainingChanged?.Invoke(this, new(TimeSpan.Zero));
                TimeRemainingElapsed?.Invoke();

                return;
            }

            // Calculate the time to wait before the next invocation of the TimeRemainingChanged event using an error
            // factor to keep the interval consistent.
            var lastError = _interval - (_stopwatch.Elapsed - lastElapsed);
            var nextDelay = _interval - lastError * 0.5;

            if (nextDelay > TimeSpan.Zero)
            {
                Thread.Sleep(nextDelay);
            }

            lastElapsed = elapsed;
        }
    }
}

/// <summary>
///     Event handler for the TimeRemainingChanged event.
/// </summary>
public delegate void TimeRemainingChangedEventHandler(object sender, TimeRemainingChangedEventArgs e);

/// <summary>
///     Event arguments for the TimeRemainingChanged event.
/// </summary>
/// <param name="timeRemaining"></param>
public class TimeRemainingChangedEventArgs(TimeSpan timeRemaining) : EventArgs
{
    /// <summary>
    ///     The time remaining until the timer reaches zero.
    /// </summary>
    public TimeSpan TimeRemaining { get; } = timeRemaining;
}