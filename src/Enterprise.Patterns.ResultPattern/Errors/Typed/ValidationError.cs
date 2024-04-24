namespace Enterprise.Patterns.ResultPattern.Errors.Typed;

public class ValidationError : Error
{
    public const string GenericCode = "ValidationError";
    public const string GenericMessage = "A validation error has occurred.";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.Validation;

    public ValidationError(
        string code = GenericCode,
        string message = GenericMessage,
        Dictionary<string, object>? metadata = null)
        : base(code, message, [Descriptor], metadata ?? [])
    {
    }
}