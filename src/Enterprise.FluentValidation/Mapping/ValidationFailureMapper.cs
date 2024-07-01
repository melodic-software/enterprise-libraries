using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Validation.Model;
using FluentValidation.Results;

namespace Enterprise.FluentValidation.Mapping;

public static class ValidationFailureMapper
{
    public static ValidationError[] MapToValidationError(ValidationFailure[] validationFailures)
    {
        ValidationError[] validationErrors = validationFailures
            .Select(validationFailure => new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .ToArray();

        return validationErrors;
    }

    public static IEnumerable<IError> MapToError(ValidationFailure[] validationFailures)
    {
        IEnumerable<IError> errors = validationFailures
            .Select(x => Error.Validation(x.ErrorCode, x.ErrorMessage))
            .ToList();

        return errors;
    }
}
