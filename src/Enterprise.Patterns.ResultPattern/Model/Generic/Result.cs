using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

/// <summary>
/// This is the generic variant of <see cref="Result"/> that specifies a typed value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public partial class Result<TValue> : Result, IResult<TValue>
{
    private const string FailedResultValueAccessErrorMessage = "Cannot access the value of a failed result.";

    private readonly TValue? _value;

    /// <summary>
    /// Gets the value of the result. 
    /// Throws InvalidOperationException if the result is a failure.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure and this property is accessed.</exception>
    [NotNull]
    public TValue Value
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

    protected internal Result(TValue? value, IEnumerable<IError> errors) : base(errors)
    {
        _value = value;
    }

    protected internal Result(TValue? value) : base([])
    {
        _value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Result{T}"/> with a value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="Result{T}"/> containing the provided value.</returns>
    public static Result<TValue> From(TValue? value)
    {
        return new Result<TValue>(value);
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static new Result<TValue> Failure(IError error) => new(default, [error]);
    public static new Result<TValue> Failure(IEnumerable<IError> errors) => new(default, errors);
    public static Result<TValue> Create(TValue? value) => value is not null ? Success(value) : Failure(Error.NullValue());

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
    public static implicit operator Result<TValue>(Error[] errors) => Failure(errors.ToList());
    public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);
}
