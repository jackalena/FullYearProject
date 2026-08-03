using System;
using System.Diagnostics;
using System.Threading;

namespace FullYearProject.Models;

public class TimeRemainingProvider
{
    private static readonly TimeSpan _interval = TimeSpan.FromSeconds(1);
    private readonly Stopwatch _stopwatch = new();
    private Thread? _timerThread;
    private volatile bool _timerThreadRunning;

    public TimeRemainingProvider()
    {
        StartTime = TimeSpan.Zero;
    }

    public TimeRemainingProvider(TimeSpan startTime)
    {
        StartTime = startTime;
    }

    public TimeSpan StartTime
    {
        get;
        set
        {
            if (_timerThreadRunning)
            {
                throw new InvalidOperationException("Cannot change StartTime while the timer is running.");
            }

            field = value;
        }
    }

    public bool AutoStart
    {
        get;
        init
        {
            field = value;
            if (value)
            {
                Start();
            }
        }
    }

    public TimeSpan TimeRemaining { get; private set; }

    public event TimeRemainingChangedEventHandler? TimeRemainingChanged;
    public event Action? TimeRemainingElapsed;

    public void Start()
    {
        TimeRemaining = StartTime;

        _stopwatch.Restart();
        _timerThreadRunning = true;

        _timerThread = new(TimerThreadTask) { IsBackground = true };
        _timerThread.Start();
    }

    public void Stop()
    {
        _timerThreadRunning = false;
        _stopwatch.Stop();
    }

    private void TimerThreadTask()
    {
        var lastElapsed = -_interval;

        while (_timerThreadRunning)
        {
            var elapsed = _stopwatch.Elapsed;

            if (elapsed < StartTime)
            {
                var roundedTime =
                    new TimeSpan((long)Math.Round((double)(StartTime - elapsed).Ticks / TimeSpan.TicksPerSecond) *
                                 TimeSpan.TicksPerSecond);

                TimeRemainingChanged?.Invoke(this, new(roundedTime));
            }
            else
            {
                TimeRemainingChanged?.Invoke(this, new(TimeSpan.Zero));
                TimeRemainingElapsed?.Invoke();
            }

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

public delegate void TimeRemainingChangedEventHandler(object sender, TimeRemainingChangedEventArgs e);

public class TimeRemainingChangedEventArgs(TimeSpan timeRemaining) : EventArgs
{
    public TimeSpan TimeRemaining { get; } = timeRemaining;
}