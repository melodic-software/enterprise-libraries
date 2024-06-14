using Enterprise.Patterns.ResultPattern.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic.Abstract;

/// <summary>
/// Represents the outcome of an operation, encapsulating success or failure information and a resulting value.
/// This interface is part of the Result pattern, often used to avoid exceptions for non-exceptional control flow,
/// and is a key component of Railway Oriented Programming (ROP).
/// </summary>
/// <typeparam name="TValue">The type of the value associated with the result.</typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// Gets the value associated with the result.
    /// </summary>
    TValue? Value { get; }
}
