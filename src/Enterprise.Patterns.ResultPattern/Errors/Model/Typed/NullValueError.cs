namespace Enterprise.Patterns.ResultPattern.Errors.Model.Typed;

public class NullValueError : Error
{
    public const string GenericCode = "NullValue";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.Validation;

    public NullValueError(
        string code = GenericCode,
        string message = "",
        IEnumerable<ErrorDescriptor>? errorDescriptors = null,
        Dictionary<string, object>? metadata = null)
        : base(code, message, errorDescriptors ?? [Descriptor], metadata ?? [])
    {
    }
}
