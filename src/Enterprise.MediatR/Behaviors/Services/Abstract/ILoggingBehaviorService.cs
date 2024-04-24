using Enterprise.Patterns.ResultPattern.Errors;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Services.Abstract;

public interface ILoggingBehaviorService
{
    public void LogApplicationServiceError(ILogger logger, IEnumerable<IError> errors, string typeName);
}