using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers;

public class FluentValidationCommandHandler<TCommand> : CommandHandlerDecoratorBase<TCommand>
    where TCommand : IBaseCommand
{
    private readonly IReadOnlyCollection<IValidator<TCommand>> _validators;
    private readonly ILogger<FluentValidationCommandHandler<TCommand>> _logger;

    public FluentValidationCommandHandler(IHandleCommand<TCommand> commandHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<FluentValidationCommandHandler<TCommand>> logger) : base(commandHandler, decoratorService)
    {
        _validators = validators.ToList();
        _logger = logger;
    }

    public override async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            _logger.LogDebug("No (fluent) validators were found for command.");
            await Decorated.HandleAsync(command, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TCommand>(command);

        _logger.LogDebug("Executing fluent validation.");
        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);
        _logger.LogDebug("Fluent validation succeeded.");

        await Decorated.HandleAsync(command, cancellationToken);
    }
}
