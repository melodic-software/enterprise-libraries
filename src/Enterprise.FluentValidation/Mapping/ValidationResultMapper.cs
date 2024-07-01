using FluentValidation.Results;

namespace Enterprise.FluentValidation.Mapping;

public static class ValidationResultMapper
{
    public static ValidationFailure[] MapToValidationFailure(IEnumerable<ValidationResult> validationResults)
    {
        ValidationFailure[] validationFailures = validationResults
            .Where(x => !x.IsValid && x is { Errors: not null } && x.Errors.Count != 0)
            .SelectMany(x => x.Errors)
            .ToArray();

        return validationFailures;
    }
}
