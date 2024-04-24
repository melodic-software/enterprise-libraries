using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.MediatR.Behaviors.Services;
using Enterprise.MediatR.Behaviors.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUseCase
    where TResponse : Result
{
    private readonly ILogger<TRequest> _logger;
    private readonly ILoggingBehaviorService _loggingBehaviorService;

    public LoggingBehavior(ILogger<TRequest> logger) : this(logger, new LoggingBehaviorService())
    {

    }

    public LoggingBehavior(ILogger<TRequest> logger, ILoggingBehaviorService loggingBehaviorService)
    {
        _logger = logger;
        _loggingBehaviorService = loggingBehaviorService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string typeName = request.GetType().Name;

        // TODO: Do we want to look at dynamic "command" vs "query" detail in the structured logs or is use case qualifier good enough?
        // This will be different in our non MediatR decorators.

        try
        {
            _logger.LogInformation("Executing \"{UseCase}\" use case.", typeName);

            TResponse result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation("\"{UseCase}\" was successful.", typeName);
            }
            else
            {
                _loggingBehaviorService.LogApplicationServiceError(_logger, result.Errors, typeName);
            }
            
            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the \"{UseCase}\" use case.", typeName);
            throw;
        }
    }
}