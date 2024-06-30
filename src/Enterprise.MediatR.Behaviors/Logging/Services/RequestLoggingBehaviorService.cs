using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging.Services;

public class RequestLoggingBehaviorService : IRequestLoggingBehaviorService
{
    public void LogApplicationServiceError(ILogger logger, IEnumerable<IError> errors, string requestTypeName)
    {
        logger.LogInformation("Request failed with errors. {@Errors}", errors);
    }
}
