using Enterprise.Logging.Core.Operations;
using Enterprise.Logging.Core.Operations.Abstract;
using Enterprise.Logging.Core.TEMP.Timing;
using Enterprise.Logging.Core.TEMP.Timing.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.TEMP.Diagnostics;

public class TimedOperation : IDisposable
{
    private readonly IStopwatch _stopwatch;
    private readonly IOperationLogger _logger;
    private readonly string _messageTemplate;
    private readonly LogLevel _logLevel;
    private readonly object[] _args;

    public TimedOperation(ILogger logger, LogLevel logLevel, string messageTemplate, params object[] args) :
        this(new StopwatchWrapper(), new OperationLogger(logger), logLevel, messageTemplate, args)
    {

    }

    public TimedOperation(IStopwatch stopWatch, IOperationLogger logger, LogLevel logLevel, string messageTemplate, params object[] args)
    {
        _stopwatch = stopWatch;
        _logger = logger;
        _logLevel = logLevel;
        _messageTemplate = messageTemplate;
        _args = args;

        _stopwatch.Start();
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        TimeSpan duration = _stopwatch.Elapsed;
        _logger.LogOperationDuration(_messageTemplate, _logLevel, duration, _args);
    }
}