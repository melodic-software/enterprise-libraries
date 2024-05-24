using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.CommandHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class FluentValidationCommandHandler<T> : CommandHandlerDecoratorBase<T>
    where T : ICommand
{
    private readonly IReadOnlyCollection<IValidator<T>> _validators;
    private readonly ILogger<FluentValidationCommandHandler<T>> _logger;

    public FluentValidationCommandHandler(IHandleCommand<T> commandHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<T>> validators,
        ILogger<FluentValidationCommandHandler<T>> logger) : base(commandHandler, decoratorService)
    {
        _validators = validators.ToList();
        _logger = logger;
    }

    public override async Task HandleAsync(T command, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            _logger.LogDebug("No (fluent) validators were found for command.");
            await Decorated.HandleAsync(command, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<T>(command);

        _logger.LogDebug("Executing fluent validation.");
        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);
        _logger.LogDebug("Fluent validation succeeded.");

        await Decorated.HandleAsync(command, cancellationToken);
    }
}
