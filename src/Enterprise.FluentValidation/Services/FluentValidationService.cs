using Enterprise.Validation.Model;
using FluentValidation;
using ValidationException = Enterprise.Exceptions.ValidationException;

namespace Enterprise.FluentValidation.Services;

public static class FluentValidationService
{
    public static void ExecuteValidation(IReadOnlyCollection<IValidator> validators, IValidationContext validationContext)
    {
        List<ValidationError> validationErrors = validators
            .Select(x => x.Validate(validationContext))
            .Where(x => x is { Errors: not null } && x.Errors.Count != 0)
            .SelectMany(x => x.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToList();

        if (!validationErrors.Any())
            return;

        // This should be handled in an upper layer - like a global error handling middleware.
        // TODO: Do we want to use an exception here or raise validation failure events and return out.

        throw new ValidationException(validationErrors);
    }
}