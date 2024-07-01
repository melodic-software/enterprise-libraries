using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services.Generic;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic;

public class FluentValidationCommandHandler<TCommand, TResult> : CommandHandlerDecoratorBase<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    private readonly IReadOnlyCollection<IValidator<TCommand>> _validators;
    private readonly ILogger<FluentValidationCommandHandler<TCommand, TResult>> _logger;

    public FluentValidationCommandHandler(IHandleCommand<TCommand, TResult> commandHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<FluentValidationCommandHandler<TCommand, TResult>> logger) : base(commandHandler, decoratorService)
    {
        _validators = validators.ToList();
        _logger = logger;
    }

    public override async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            _logger.LogDebug("No (fluent) validators were found for command.");
            return await Decorated.HandleAsync(command, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TCommand>(command);

        _logger.LogDebug("Executing fluent validation.");
        TResult? result = await FluentValidationService.ExecuteValidationAsync<TResult>(_validators, validationContext);
        _logger.LogDebug("Fluent validation completed.");

        if (!Equals(result, default(TResult)))
        {
            return result;
        }

        return await Decorated.HandleAsync(command, cancellationToken);
    }
}
