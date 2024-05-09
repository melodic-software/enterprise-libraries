using Enterprise.Validation.Model;

namespace Enterprise.Validation.Exceptions;

/// <summary>
/// This is to primarily be used for data type and format validation.
/// It should not be used for domain level validation errors (business rules).
/// One example of proper use is an application service decorator that uses fluent validation for validating command/query objects.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException() : this([])
    {

    }

    public ValidationException(IReadOnlyCollection<ValidationError>? validationErrors)
    {
        ValidationErrors = validationErrors ?? [];
    }

    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
}
