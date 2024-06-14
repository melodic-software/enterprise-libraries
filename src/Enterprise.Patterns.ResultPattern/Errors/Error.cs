using Enterprise.Patterns.ResultPattern.Errors.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Typed;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Errors;

/// <summary>
/// An error that has occurred in the domain layer, or one that has been captured and translated in the application service layer.
/// This error class can be used with factory creation methods along with the Result pattern objects.
/// Derived types can be created, but may create too much additional complexity or overhead.
/// A base for each descriptor can be applied if desired.
/// </summary>
public class Error : IError
{
    internal const string DefaultCode = "Unknown";

    /// <summary>
    /// A short human-readable code for the error.
    /// Examples: "User.NotFound", "Payment.NotReceived", "Subscription.Inactive", etc.
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// The human-readable message explaining the error.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// One or more descriptors for the type, quality, and kind of error.
    /// Most errors typically have only one.
    /// This can help application components in the presentation layer present the best possible response.
    /// </summary>
    public IEnumerable<ErrorDescriptor> Descriptors { get; init; }

    /// <summary>
    /// An optional dictionary for passing along additional information.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; }

    internal Error(string? code, string? message, IEnumerable<ErrorDescriptor>? errorDescriptors, Dictionary<string, object>? metadata = null)
    {
        code = code?.Trim();
        message = message?.Trim();

        Code = string.IsNullOrWhiteSpace(code) ? DefaultCode : code;
        Message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
        Descriptors = errorDescriptors?.ToList() ?? [];
        Metadata = metadata?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// This uses the "null object" pattern to represent a non error.
    /// </summary>
    /// <returns></returns>
    public static NoError None() => new();
    public static NullValueError NullValue() => new();
    public static NullValueError NullValue(string code, string message) => new(code, message);
    public static NotFoundError NotFound() => new();
    public static NotFoundError NotFound(string code, string message) => new(code, message);
    public static ValidationError Validation(string message) => new(ValidationError.GenericCode, message);
    public static ValidationError Validation(string code, string message) => new(code, message);
    public static BusinessRuleViolation BusinessRuleViolation(string code, string message) => new(code, message);
    public static ConflictError Conflict(string code, string message) => new(code, message);
    public static PermissionError Permission() => new();
    public static PermissionError Permission(string code, string message) => new(code, message);

    public static Error Custom(string code, string message, ErrorDescriptor errorDescriptor, Dictionary<string, object>? metadata = null)
        => Custom(code, message, [errorDescriptor], metadata);

    public static Error Custom(string code, string message, IEnumerable<ErrorDescriptor> errorDescriptors, Dictionary<string, object>? metadata = null)
        => new(code, message, errorDescriptors, metadata);

    public Result ToResult => Result.Failure(this);

    public override string ToString()
    {
        string result = Code;
        
        if (!string.IsNullOrWhiteSpace(Message))
        {
            result += $" - {Message}";
        }

        return result;
    }
}
