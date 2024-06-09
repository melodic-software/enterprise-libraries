using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.FluentValidation.Services;
using FluentValidation;
using MediatR;

namespace Enterprise.MediatR.Behaviors.Validation;

// TODO: Can we consolidate this with the other logging decorator(s)?
public class CommandFluentValidationBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : ICommand
{
    private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;

    public CommandFluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToList();
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        IValidationContext validationContext = new ValidationContext<TRequest>(request);

        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);

        return await next();
    }
}
