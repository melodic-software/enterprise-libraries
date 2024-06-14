namespace Enterprise.Patterns.ResultPattern.Errors.Model.Typed;

/// <summary>
/// These are exceptions that have been caught and captured as an error.
/// These should rarely be used and only if an exception is expected, and if it is possible to recover from it.
/// Use with caution.
/// </summary>
public class ExceptionError : Error
{
    public const string GenericCode = "Exception";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.Exception;
    public const string ExceptionMetadataDictionaryKey = "Exception";

    public Exception Exception { get; }

    public ExceptionError(Exception exception, string code = GenericCode) :
        base(code, exception.Message, [Descriptor], CreateMetadataDictionary(exception))
    {
        Exception = exception;
    }

    private static Dictionary<string, object> CreateMetadataDictionary(Exception exception) => new()
    {
        { ExceptionMetadataDictionaryKey, exception }
    };
}
