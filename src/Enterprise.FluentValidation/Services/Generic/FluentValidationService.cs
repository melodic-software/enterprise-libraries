using System.Reflection;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using Enterprise.Validation.Model;
using FluentValidation;
using FluentValidation.Results;
using static Enterprise.FluentValidation.Mapping.ValidationFailureMapper;
using static Enterprise.FluentValidation.Mapping.ValidationResultMapper;
using ValidationException = Enterprise.Validation.Exceptions.ValidationException;

namespace Enterprise.FluentValidation.Services.Generic;

public static class FluentValidationService
{
    public static async Task<TResult> ExecuteValidationAsync<TResult>(IReadOnlyCollection<IValidator> validators, IValidationContext validationContext)
    {
        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(validationContext)));
        TResult? result = ProcessResults<TResult>(validationResults);
        return result;
    }

    public static TResult? ExecuteValidation<TResult>(IReadOnlyCollection<IValidator> validators, IValidationContext validationContext)
    {
        IEnumerable<ValidationResult> validationResults = validators.Select(x => x.Validate(validationContext));
        TResult? result = ProcessResults<TResult>(validationResults);
        return result;
    }

    private static TResult? ProcessResults<TResult>(IEnumerable<ValidationResult> validationResults)
    {
        ValidationFailure[] validationFailures = MapToValidationFailure(validationResults);

        if (!validationFailures.Any())
        {
            return default;
        }

        Type resultType = typeof(TResult);

        if (resultType.IsGenericType &&
            resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type genericResultTypeArg = resultType.GetGenericArguments()[0];

            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(genericResultTypeArg)
                .GetMethod(nameof(Result<object>.Failure))
                ?.MakeGenericMethod(genericResultTypeArg);

            if (failureMethod is not null)
            {
                IEnumerable<IError> errors = MapToError(validationFailures);
                return (TResult)failureMethod.Invoke(null, [errors]);
            }
        }
        else if (resultType == typeof(Result))
        {
            IEnumerable<IError> errors = MapToError(validationFailures);
            return (TResult)(object)Result.Failure(errors);
        }

        // We translate from a fluent validation object to a core enterprise validation error model.
        ValidationError[] validationErrors = MapToValidationError(validationFailures);

        // Raise our own exception instead of a library specific exception.
        throw new ValidationException(validationErrors);
    }
}
