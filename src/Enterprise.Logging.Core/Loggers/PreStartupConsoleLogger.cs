using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Loggers;

/// <summary>
/// A simple console logger used by the PreStartupLogger to log messages before the main logger is available.
/// Supports a basic form of structured logging.
/// </summary>
internal class PreStartupConsoleLogger : ILogger
{
    private readonly string _categoryName;
    private readonly LogLevel _minLogLevel;

    internal PreStartupConsoleLogger(string categoryName, LogLevel minLogLevel = LogLevel.Information)
    {
        _categoryName = categoryName;
        _minLogLevel = minLogLevel;
    }

    /// <summary>
    /// Logs the specified message at the given log level.
    /// </summary>
    /// <param name="logLevel">The severity level of the log message.</param>
    /// <param name="eventId">The event ID of the log message.</param>
    /// <param name="state">The state associated with the log message.</param>
    /// <param name="exception">The exception associated with the log message, if any.</param>
    /// <param name="formatter">The function to create a string message from the state and exception.</param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        string formattedMessage = formatter(state, exception);



        Console.WriteLine($"{DateTime.UtcNow:O} [{_categoryName}] {logLevel}: {formattedMessage}");
    }

    /// <summary>
    /// Checks if the given log level is enabled.
    /// </summary>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns>True if logging is enabled for the specified log level, otherwise false.</returns>
    public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

    /// <summary>
    /// Begins a logical operation scope.
    /// </summary>
    /// <param name="state">The state to use for the scope.</param>
    /// <returns>A disposable object that ends the logical operation scope on dispose.</returns>
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;
}