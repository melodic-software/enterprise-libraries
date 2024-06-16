using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

/// <summary>
/// These are extension methods that deal with singular errors.
/// </summary>
public static class ErrorExtensions
{
    public static bool IsTrueError(this IError error) => error.Descriptors.ContainTrueError();
    public static Result ToResult(this IError error) => Result.Failure(error);
    public static Result<T> ToResult<T>(this IError error) => Result<T>.Failure(error);
}
