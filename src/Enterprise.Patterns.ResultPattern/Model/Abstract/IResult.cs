using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Abstract;

/// <summary>
/// Represents the outcome of an operation, encapsulating success or failure information.
/// This interface is part of the Result pattern, often used to avoid exceptions for non-exceptional control flow,
/// and is a key component of Railway Oriented Programming (ROP).
/// </summary>
public interface IResult
{
    /// <summary>
    /// A collection of errors that may have occurred during execution.
    /// </summary>
    List<IError> Errors { get; }

    /// <summary>
    /// Was the operation was successful?
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Did the operation fail?
    /// </summary>
    bool IsFailure => !IsSuccess;
}
