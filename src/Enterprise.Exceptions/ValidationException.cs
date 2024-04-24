using Enterprise.Validation.Model;

namespace Enterprise.Exceptions;

/// <summary>
/// This is to primarily be used for data type and format validation.
/// It should not be used for domain level validation errors (business rules).
/// One example of proper use is an application service decorator that uses fluent validation for validating command/query objects.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(IReadOnlyCollection<ValidationError>? validationErrors)
    {
        ValidationErrors = validationErrors ?? new List<ValidationError>();
    }

    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
}