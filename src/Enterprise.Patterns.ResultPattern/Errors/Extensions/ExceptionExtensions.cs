using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

public static class ExceptionExtensions
{
    public static ExceptionError ToError(this Exception exception)
    {
        return new ExceptionError(exception);
    }

    public static Result ToResult(this Exception exception)
    {
        ExceptionError error = exception.ToError();
        return Result.Failure(error);
    }

    public static Result ToResult(this ExceptionError error)
    {
        return Result.Failure(error);
    }

    public static Result<TValue> ToResult<TValue>(this Exception exception)
    {
        ExceptionError error = exception.ToError();
        return Result<TValue>.Failure(error);
    }

    public static Result<TValue> ToResult<TValue>(this ExceptionError error)
    {
        return Result<TValue>.Failure(error);
    }
}
