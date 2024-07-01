using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.FluentValidation.Services.Generic;
using FluentValidation;
using MediatR;

namespace Enterprise.MediatR.Behaviors.Validation;

/// <summary>
/// Executes any registered validator implementations from the FluentValidation library on command requests.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class CommandFluentValidationBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult> where TRequest : IUseCase
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

        TResult? result = await FluentValidationService.ExecuteValidationAsync<TResult>(_validators, validationContext);

        if (!Equals(result, default(TResult)))
        {
            return result;
        }

        return await next();
    }
}
