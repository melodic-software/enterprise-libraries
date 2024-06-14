using Enterprise.Patterns.ResultPattern.Errors.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

public static class ErrorExtensions
{
    public static bool IsTrueError(this IError error) => error.Descriptors.ContainTrueError();
    public static Result<T> ToResult<T>(this IError error) => Result.Failure<T>(error);
}
