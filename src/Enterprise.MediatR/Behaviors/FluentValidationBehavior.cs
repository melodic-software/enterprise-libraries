﻿using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.FluentValidation.Services;
using FluentValidation;
using MediatR;

namespace Enterprise.MediatR.Behaviors;

// TODO: Can we consolidate this with the other logging decorator(s)?
public class FluentValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;

    public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToList();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        IValidationContext validationContext = new ValidationContext<TRequest>(request);

        FluentValidationService.ExecuteValidation(_validators, validationContext);

        return await next();
    }
}