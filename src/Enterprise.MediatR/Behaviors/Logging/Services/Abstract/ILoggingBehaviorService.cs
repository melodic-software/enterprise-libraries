﻿using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging.Services.Abstract;

public interface ILoggingBehaviorService
{
    public void LogApplicationServiceError(ILogger logger, IEnumerable<IError> errors, string typeName);
}
