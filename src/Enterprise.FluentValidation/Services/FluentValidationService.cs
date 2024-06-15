using Enterprise.Validation.Model;
using FluentValidation;
using FluentValidation.Results;
using ValidationException = Enterprise.Validation.Exceptions.ValidationException;

namespace Enterprise.FluentValidation.Services;

public static class FluentValidationService
{
    public static async Task ExecuteValidationAsync(IReadOnlyCollection<IValidator> validators, IValidationContext validationContext)
    {
        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(validationContext)));

        ProcessResults(validationResults);
    }

    public static void ExecuteValidation(IReadOnlyCollection<IValidator> validators, IValidationContext validationContext)
    {
        IEnumerable<ValidationResult> validationResults = validators.Select(x => x.Validate(validationContext));

        ProcessResults(validationResults);
    }

    private static void ProcessResults(IEnumerable<ValidationResult> validationResults)
    {
        IEnumerable<ValidationFailure> validationFailures = validationResults
            .Where(x => x is { Errors: not null } && x.Errors.Count != 0)
            .SelectMany(x => x.Errors);

        var validationErrors = validationFailures
            .Select(validationFailure => new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .ToList();

        if (!validationErrors.Any())
        {
            return;
        }

        // This should be handled in an upper layer - like a global error handling middleware.
        // TODO: Do we want to use an exception here or raise validation failure events and return out.

        throw new ValidationException(validationErrors);
    }
}
