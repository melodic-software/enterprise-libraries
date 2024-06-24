using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Enterprise.FluentValidation.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public class FluentValidationCommandHandler<TCommand, TResult> : IHandler<TCommand, TResult>
{
    private readonly IReadOnlyCollection<IValidator<TCommand>> _validators;
    private readonly ILogger<FluentValidationCommandHandler<TCommand, TResult>> _logger;

    public FluentValidationCommandHandler(
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<FluentValidationCommandHandler<TCommand, TResult>> logger)
    {
        _validators = validators.ToList();
        _logger = logger;
    }

    public async Task<TResult?> HandleAsync(TCommand request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            _logger.LogDebug("No (fluent) validators were found for command.");
            return await next();
        }

        IValidationContext validationContext = new ValidationContext<TCommand>(request);

        _logger.LogDebug("Executing fluent validation.");
        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);
        _logger.LogDebug("Fluent validation succeeded.");

        return await next();
    }
}
