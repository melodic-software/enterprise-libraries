namespace Enterprise.Patterns.ResultPattern.Errors.Typed;

public class NotFoundError : Error
{
    public const string GenericCode = "NotFound";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.NotFound;

    public NotFoundError(
        string code = GenericCode,
        string message = "",
        Dictionary<string, object>? metadata = null)
        : base(code, message, [Descriptor], metadata ?? [])
    {
    }
}
