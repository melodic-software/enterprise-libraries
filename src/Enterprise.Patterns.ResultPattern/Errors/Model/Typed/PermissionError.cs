using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Errors.Model.Typed;

public class PermissionError : Error
{
    public const string GenericCode = "PermissionError";
    public const string GenericMessage = "A permission error has occurred.";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.Validation;

    public PermissionError(
        string code = GenericCode,
        string message = GenericMessage,
        Dictionary<string, object>? metadata = null)
        : base(code, message, [Descriptor], metadata ?? [])
    {
    }
}
