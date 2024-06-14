using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

/// <summary>
/// This is the generic variant of <see cref="Result"/> that specifies a typed value.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class Result<T> : Result, IResult<T>
{
    private const string FailedResultValueAccessErrorMessage = "Cannot access the value of a failed result.";

    private readonly T? _value;

    /// <summary>
    /// Gets the value of the result. 
    /// Throws InvalidOperationException if the result is a failure.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure and this property is accessed.</exception>
    [NotNull]
    public T Value
    {
        get
        {
            if (IsSuccess)
            {
                return _value!;
            }

            // Building a detailed error message if the result is a failure.
            var errorMessageBuilder = new StringBuilder(FailedResultValueAccessErrorMessage);

            if (Errors.Any())
            {
                errorMessageBuilder.Append(" Errors: ");
            }

            foreach (IError error in Errors)
            {
                errorMessageBuilder.AppendLine(CultureInfo.InvariantCulture, $"{error.Code} - {error.Message}");
            }

            throw new InvalidOperationException(errorMessageBuilder.ToString());
        }
    }

    protected internal Result(T? value, IEnumerable<IError> errors)
        : base(errors)
    {
        _value = value;
    }

    protected internal Result(T? value) : base([])
    {
        _value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Result{T}"/> with a value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="Result{T}"/> containing the provided value.</returns>
    public static Result<T> From(T? value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(T? value) => Create(value);
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
    public static implicit operator Result<T>(Error[] errors) => Failure<T>(errors.ToList());
    public static implicit operator Result<T>(List<Error> errors) => Failure<T>(errors);
}
