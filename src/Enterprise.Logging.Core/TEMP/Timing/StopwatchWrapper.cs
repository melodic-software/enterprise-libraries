using System.Diagnostics;
using Enterprise.Logging.Core.TEMP.Timing.Abstractions;

namespace Enterprise.Logging.Core.TEMP.Timing;

public class StopwatchWrapper : IStopwatch
{
    private readonly Stopwatch _stopwatch;

    public StopwatchWrapper() : this(new())
    {

    }

    public StopwatchWrapper(Stopwatch stopwatch)
    {
        _stopwatch = stopwatch;
    }

    public TimeSpan Elapsed => _stopwatch.Elapsed;

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Reset()
    {
        _stopwatch.Reset();
    }
}