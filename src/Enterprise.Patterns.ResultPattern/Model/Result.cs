using Enterprise.Patterns.ResultPattern.Abstract;
using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Enterprise.Patterns.ResultPattern.Model;

/// <summary>
/// Represents a generic operation result, encapsulating success or failure, with associated error information.
/// This is a specific pattern to help mitigate the use of exceptions for things that aren't really exceptional system behavior.
/// These are mostly used in the domain and application service layers, and are passed back to the presentation layer.
/// </summary>
public class Result : IResult
{
    private readonly IEnumerable<IError> _errors;

    public bool IsSuccess => !HasErrors;
    public bool IsFailure => !IsSuccess;
    public List<IError> Errors => _errors.GetTrueErrors();
    public bool HasErrors => Errors.HasTrueError();
    public IError FirstError => Errors.FirstOrDefault(x => x.IsTrueError()) ?? Errors.FirstOrDefault() ?? Error.None();
    
    protected internal Result(IEnumerable<IError> errors)
    {
        IEnumerable<IError> filtered = ErrorFilterService.FilterInvalid(errors);

        _errors = filtered;
    }

    public static Result Success() => new([]);
    public static Result<T> Success<T>(T value) => new(value);
    public static Result Failure(IError error) => new([error]);
    public static Result Failure(IEnumerable<IError> errors) => new(errors);
    public static Result<T> Failure<T>(IError error) => new(default, [error]);
    public static Result<T> Failure<T>(IEnumerable<IError> errors) => new(default, errors);
    public static Result<T> Create<T>(T? value) =>
        value is not null ? Success(value) : Failure<T>(Error.NullValue());

    public static implicit operator Result(Error error) => Failure(error);
    public static implicit operator Result(Error[] errors) => Failure(errors.ToList());
    public static implicit operator Result(List<Error> errors) => Failure(errors);

    public override string ToString()
    {
        string result = $"{nameof(IsSuccess)}: {IsSuccess}";

        if (Errors.Any())
            result += $" Error(s): {Errors.Count}";

        return result;
    }
}

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
                return _value!;

            // Building a detailed error message if the result is a failure.
            StringBuilder errorMessageBuilder = new StringBuilder(FailedResultValueAccessErrorMessage);

            if (Errors.Any())
                errorMessageBuilder.Append(" Errors: ");

            foreach (IError error in Errors)
                errorMessageBuilder.AppendLine($"{error.Code} - {error.Message}");

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
