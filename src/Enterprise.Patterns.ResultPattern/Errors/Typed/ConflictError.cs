namespace Enterprise.Patterns.ResultPattern.Errors.Typed;

public class ConflictError : Error
{
    public const string GenericCode = "Conflict";
    public const string GenericMessage = "A conflict error has occurred.";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.Conflict;

    public ConflictError(
        string code = GenericCode,
        string message = GenericMessage,
        Dictionary<string, object>? metadata = null)
        : base(code, message, [Descriptor], metadata ?? [])
    {
    }
}
