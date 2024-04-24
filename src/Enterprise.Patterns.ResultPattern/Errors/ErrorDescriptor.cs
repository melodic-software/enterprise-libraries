namespace Enterprise.Patterns.ResultPattern.Errors;

/// <summary>
/// Describes the type, quality, and kind of error.
/// </summary>
public enum ErrorDescriptor
{
    /// <summary>
    /// This is used as a default value to represent the absence of an error.
    /// The "null object" pattern can be applied with an error instance with this descriptor, which helps reduce null checks.
    /// </summary>
    NoError,

    /// <summary>
    /// These errors occur when client input does not meet validation rules.
    /// Examples: format errors, range violations, missing required fields, etc.
    /// </summary>
    Validation,

    /// <summary>
    /// These errors occur when a user or system component attempts to perform an operation
    /// for which they do not have the necessary permissions or rights.
    /// They're forbidden to take the action.
    /// </summary>
    Permission,

    /// <summary>
    /// These errors occur when an operation violates domain-specific business rules or logic.
    /// These are typically used to enforce invariants within the domain.
    /// </summary>
    BusinessRule,

    /// <summary>
    /// This error occurs when required entities or resources are not found.
    /// This is commonly used in scenarios like database lookups.
    /// </summary>
    NotFound,

    /// <summary>
    /// These errors are specific conflicts like concurrency, or uniqueness constraints.
    /// </summary>
    Conflict
}