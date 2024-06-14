using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

public static class ExceptionExtensions
{
    public static ExceptionError ToError(this Exception exception) => new(exception);

    public static Result<T> ToResult<T>(this Exception exception)
    {
        ExceptionError error = exception.ToError();
        return Result<T>.Failure(error);
    }

    public static Result<T> ToResult<T>(this ExceptionError error)
    {
        return Result<T>.Failure(error);
    }
}
