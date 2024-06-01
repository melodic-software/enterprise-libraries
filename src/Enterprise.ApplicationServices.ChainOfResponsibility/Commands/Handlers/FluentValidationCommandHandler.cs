using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.FluentValidation.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public class FluentValidationCommandHandler<TCommand, TResponse> : IHandler<TCommand>, IHandler<TCommand, TResponse>
{
    private readonly IReadOnlyCollection<IValidator<TCommand>> _validators;
    private readonly ILogger<FluentValidationCommandHandler<TCommand, TResponse>> _logger;

    public FluentValidationCommandHandler(
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<FluentValidationCommandHandler<TCommand, TResponse>> logger)
    {
        _validators = validators.ToList();
        _logger = logger;
    }

    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            _logger.LogDebug("No (fluent) validators were found for command.");
            await next();
            return;
        }

        IValidationContext validationContext = new ValidationContext<TCommand>(request);

        _logger.LogDebug("Executing fluent validation.");
        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);
        _logger.LogDebug("Fluent validation succeeded.");

        await next();
    }

    public Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
