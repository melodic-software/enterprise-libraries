using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services;
using Enterprise.Patterns.ResultPattern.Model;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;

public class FluentValidationCommandHandler<TCommand, TResponse> : CommandHandlerDecoratorBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly IReadOnlyCollection<IValidator<TCommand>> _validators;
    private readonly ILogger<FluentValidationCommandHandler<TCommand, TResponse>> _logger;

    public FluentValidationCommandHandler(IHandleCommand<TCommand, TResponse> commandHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<FluentValidationCommandHandler<TCommand, TResponse>> logger) : base(commandHandler, decoratorService)
    {
        _validators = validators.ToList();
        _logger = logger;
    }

    public override async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            _logger.LogDebug("No (fluent) validators were found for command.");
            return await Decorated.HandleAsync(command, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TCommand>(command);

        _logger.LogDebug("Executing fluent validation.");
        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);
        _logger.LogDebug("Fluent validation succeeded.");

        return await Decorated.HandleAsync(command, cancellationToken);
    }
}
