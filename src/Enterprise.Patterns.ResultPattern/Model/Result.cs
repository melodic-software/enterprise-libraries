using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Model;

/// <summary>
/// Represents a generic operation result, encapsulating success or failure, with associated error information.
/// This is a specific pattern to help mitigate the use of exceptions for things that aren't really exceptional system behavior.
/// These are mostly used in the domain and application service layers, and are passed back to the presentation layer.
/// This is a core component (monad) in railway oriented programming (ROP) - a functional programming design pattern.
/// </summary>
public class Result : IResult
{
    private readonly IEnumerable<IError> _errors;

    public bool IsSuccess => !HasErrors;
    public bool IsFailure => !IsSuccess;
    public List<IError> Errors => _errors.GetTrueErrors();
    public bool HasErrors => Errors.HasTrueError();
    public IError FirstError => Errors.Find(x => x.IsTrueError()) ?? Errors.FirstOrDefault() ?? Error.None();
    
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
    public static Result<T> Create<T>(T? value) => value is not null ? Success(value) : Failure<T>(Error.NullValue());

    public static implicit operator Result(Error error) => Failure(error);
    public static implicit operator Result(Error[] errors) => Failure(errors.ToList());
    public static implicit operator Result(List<Error> errors) => Failure(errors);

    public override string ToString()
    {
        string result = $"{nameof(IsSuccess)}: {IsSuccess}";

        if (Errors.Any())
        {
            result += $" Error(s): {Errors.Count}";
        }

        return result;
    }
}


