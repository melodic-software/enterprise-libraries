﻿using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Errors;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging.Services;

public class LoggingBehaviorService : ILoggingBehaviorService
{
    public void LogApplicationServiceError(ILogger logger, IEnumerable<IError> errors, string typeName)
    {
        logger.LogInformation("\"{UseCase}\" failed with errors.", typeName);
    }
}
