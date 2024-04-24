using Microsoft.Extensions.Logging;

namespace Enterprise.Serilog.Startup;

internal class PreStartupSerilogWrapper : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
    
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        global::Serilog.ILogger logger = global::Serilog.Log.Logger;

        string formattedMessage = formatter.Invoke(state, exception);

        switch (logLevel)
        {
            case LogLevel.Trace:
                logger.Verbose(formattedMessage);
                break;
            case LogLevel.Debug:
                logger.Debug(formattedMessage);
                break;
            case LogLevel.Information:
                logger.Information(formattedMessage);
                break;
            case LogLevel.Warning:
                logger.Warning(formattedMessage);
                break;
            case LogLevel.Error:
                logger.Error(formattedMessage, exception);
                break;
            case LogLevel.Critical:
                logger.Error(formattedMessage, exception);
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
        }
    }
}