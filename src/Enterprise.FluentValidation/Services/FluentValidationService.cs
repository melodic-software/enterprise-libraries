using Enterprise.Validation.Model;
using FluentValidation;
using FluentValidation.Results;
using static Enterprise.FluentValidation.Mapping.ValidationFailureMapper;
using static Enterprise.FluentValidation.Mapping.ValidationResultMapper;
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
        ValidationFailure[] validationFailures = MapToValidationFailure(validationResults);

        if (!validationFailures.Any())
        {
            return;
        }

        // We translate from a fluent validation object to a core enterprise validation error model.
        ValidationError[] validationErrors = MapToValidationError(validationFailures);

        // Raise our own exception instead of a library specific exception.
        throw new ValidationException(validationErrors);
    }
}
