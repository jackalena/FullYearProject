using System;
using System.Diagnostics;
using System.Threading;

namespace FullYearProject.Models;

public class TimeRemainingProvider
{
    private static TimeSpan _interval = TimeSpan.FromSeconds(1);
    private readonly Stopwatch _stopwatch = new();

    private Thread? _timerThread;

    public TimeRemainingProvider(TimeSpan startTime)
    {
        StartTime = startTime;
    }

    public TimeSpan StartTime { get; }
    public TimeSpan TimeRemaining { get; private set; }

    public event TimeRemainingChangedEventHandler? TimeRemainingChanged;

    public event Action? TimeRemainingElapsed;

    public void Start()
    {
        TimeRemaining = StartTime;

        _stopwatch.Start();
        _timerThread = new(TimerThreadTask);
    }

    private void TimerThreadTask()
    {
        TimeSpan lastElapsed = new(0);

        while (true)
        {
        }
    }
}

public delegate void TimeRemainingChangedEventHandler(object sender, TimeRemainingChangedEventArgs e);

public class TimeRemainingChangedEventArgs : EventArgs
{
    public TimeRemainingChangedEventArgs(TimeSpan timeRemaining)
    {
    }
}