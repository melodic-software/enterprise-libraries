namespace Enterprise.Logging.Core.TEMP.Timing.Abstractions;

// TODO: Move this to a timing library.

public interface IStopwatch
{
    TimeSpan Elapsed { get; }
    void Start();
    void Stop();
    void Reset();
}