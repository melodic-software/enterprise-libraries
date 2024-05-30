using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.MediatR.Behaviors.Logging.Services;
using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging;

public class UseCaseLoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUseCase
{
    private readonly ILogger<UseCaseLoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ILoggingBehaviorService _loggingBehaviorService;

    public UseCaseLoggingBehavior(ILogger<UseCaseLoggingBehavior<TRequest, TResponse>> logger) : this(logger, new LoggingBehaviorService())
    {

    }

    public UseCaseLoggingBehavior(ILogger<UseCaseLoggingBehavior<TRequest, TResponse>> logger, ILoggingBehaviorService loggingBehaviorService)
    {
        _logger = logger;
        _loggingBehaviorService = loggingBehaviorService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        string typeName = request.GetType().Name;

        // TODO: Do we want to look at dynamic "command" vs "query" detail in the structured logs or is use case qualifier good enough?
        // This will be different in our non MediatR decorators.

        try
        {
            _logger.LogInformation("Executing \"{UseCase}\" use case.", typeName);
            TResponse response = await next();
            _logger.LogInformation("\"{UseCase}\" execution completed.", typeName);

            HandleResult(response, typeName);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the \"{UseCase}\" use case.", typeName);
            throw;
        }
    }

    private void HandleResult(TResponse response, string typeName)
    {
        if (response is not Result result)
        {
            return;
        }

        if (result.IsSuccess)
        {
            _logger.LogInformation("\"{UseCase}\" was successful.", typeName);
        }
        else
        {
            _loggingBehaviorService.LogApplicationServiceError(_logger, result.Errors, typeName);
        }
    }
}
