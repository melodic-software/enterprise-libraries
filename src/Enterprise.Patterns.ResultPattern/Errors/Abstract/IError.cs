namespace Enterprise.Patterns.ResultPattern.Errors.Abstract;

/// <summary>
/// An error that has occurred within the context of the domain.
/// These are typically aggregated and raised in the application service layer.
/// </summary>
public interface IError
{
    /// <summary>
    /// A constant descriptor for the error.
    /// Examples: "Entity.NotFound", "User.AlreadyRegistered", "Schedule.Conflict", etc.
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// The human-readable message that describes the validation issue.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// One or more error types that describe the error.
    /// </summary>
    public IEnumerable<ErrorDescriptor> Descriptors { get; }
}
