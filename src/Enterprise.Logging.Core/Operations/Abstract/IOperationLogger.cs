using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Operations.Abstract;

public interface IOperationLogger
{
    void LogOperationDuration(string messageTemplate, LogLevel logLevel, TimeSpan duration, params object[] args);
}