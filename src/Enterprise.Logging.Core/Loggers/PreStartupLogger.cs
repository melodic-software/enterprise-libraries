using Microsoft.Extensions.Logging;
using System.Text;

namespace Enterprise.Logging.Core.Loggers;

/// <summary>
/// A logger used during the application startup phase before the dependency injection container is fully set up.
/// This logger should only be used before the service provider/application has been built.
/// For logging after the application startup, resolve an <see cref="ILogger"/> instance through the DI container.
/// </summary>
public class PreStartupLogger : ILogger
{
    private readonly PreStartupConsoleLogger _logger;
    private ILogger? _diManagedLogger;
    private readonly object _syncLock = new();

    private static readonly Lazy<PreStartupLogger> Lazy = new(() => new PreStartupLogger());

    /// <summary>
    /// Gets the singleton instance of the PreStartupLogger.
    /// Warning: This logger is intended for use only during the initial startup phase of the application.
    /// Once the application and its service provider are fully built, the preferred approach is to resolve an ILogger from the DI container.
    /// </summary>
    public static PreStartupLogger Instance => Lazy.Value;

    private PreStartupLogger()
    {
        _logger = new PreStartupConsoleLogger(nameof(PreStartupLogger));
    }

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_syncLock)
        {
            if (_diManagedLogger != null)
            {
                _diManagedLogger.Log(logLevel, eventId, state, exception, formatter);
            }
            else
            {
                // For the simple console logger, we directly format the message using the provided arguments.
                // The formatter is used to handle exceptions if any, and to format the message correctly.
                string Formatter(TState msg, Exception? ex)
                {
                    string formattedMessage = formatter.Invoke(state, ex);
                    formattedMessage += FormatException(ex);
                    return formattedMessage;
                }

                _logger.Log(logLevel, eventId, state, exception, Formatter);
            }
        }
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        lock (_syncLock)
        {
            return _diManagedLogger?.IsEnabled(logLevel) ?? _logger.IsEnabled(logLevel);
        }
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        lock (_syncLock)
        {
            return _diManagedLogger?.BeginScope(state) ?? _logger.BeginScope(state);
        }
    }

    /// <summary>
    /// Sets the DI-managed ILogger to be used by the PreStartupLogger, transitioning from the initial
    /// console logging to using the fully configured logger provided by the DI container.
    /// </summary>
    /// <param name="logger">The ILogger instance to use for logging.</param>
    public void SetLogger(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        lock (_syncLock)
        {
            _diManagedLogger = logger;
        }
    }

    private string FormatException(Exception? ex)
    {
        if (ex == null)
            return string.Empty;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"\nException: {ex.Message}");
        stringBuilder.AppendLine($"Type: {ex.GetType().FullName}");
        stringBuilder.AppendLine($"Stack Trace: {ex.StackTrace}");

        if (ex.InnerException == null)
            return stringBuilder.ToString();

        stringBuilder.AppendLine("Inner Exception:");

        // Recursive call for inner exceptions.
        stringBuilder.Append(FormatException(ex.InnerException));  

        return stringBuilder.ToString();
    }
}