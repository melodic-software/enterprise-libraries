using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Diagnostics;
// TODO: This mixes diagnostic / timing behavior and logging.
// It might be best to split these out in the future,
// and create a setup so the two are working together but have no knowledge of each other (direct coupling).

// Also, abstractions could be used for the stopwatch (like an ITimer).
// The logic for capturing the start time, and the delta could be moved there.
// TODO: Create timing / diagnostic library, migrate timing logic, and reference abstractions here.

public class TimedOperation : IDisposable
{
    private readonly ILogger _logger;
    private readonly LogLevel _logLevel;
    private readonly string _messageTemplate;
    private readonly object?[] _args;
    private readonly long _startingTimestamp;

    /// <summary>
    /// Initializes a new instance of the TimedOperation class, starting the timing process.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="args"></param>
    public TimedOperation(ILogger logger, LogLevel logLevel, string messageTemplate, object?[] args)
    {
        _logger = logger;
        _logLevel = logLevel;
        _messageTemplate = messageTemplate;

        // Initialize the arguments array with an extra slot for the operation duration.
        _args = new object[args.Length + 1];
        Array.Copy(args, _args, args.Length); // Copy existing arguments into the new array.

        _startingTimestamp = Stopwatch.GetTimestamp();
    }

    public void Dispose()
    {
        // Calculate the elapsed time since the start of the operation.
        TimeSpan delta = Stopwatch.GetElapsedTime(_startingTimestamp);

        // Set the last argument to the elapsed time in milliseconds.
        _args[^1] = delta.TotalMilliseconds;

        // Log the operation's duration using the provided log level and message template.
        _logger.Log(_logLevel, $"{_messageTemplate} completed in {{OperationDurationMs}}ms", _args);
    }
}