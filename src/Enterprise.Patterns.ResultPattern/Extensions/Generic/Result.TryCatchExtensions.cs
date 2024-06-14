using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    /// <summary>
    /// Attempts to apply a function to the value of a successful result, catching any exceptions that occur.
    /// If an exception is thrown, a failure result with the provided error is returned.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value.</typeparam>
    /// <typeparam name="TOut">The type of the output value.</typeparam>
    /// <param name="result">The initial result object.</param>
    /// <param name="func">The function to apply if the initial result is successful. This function may throw an exception.</param>
    /// <param name="error">The error to return if an exception is thrown.</param>
    /// <returns>A new result object with the function's output or a failure result with the provided error if an exception is thrown.</returns>
    public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func, Error error)
    {
        try
        {
            return result.IsSuccess ?
                Result<TOut>.Success(func(result.Value)) :
                Result<TOut>.Failure(result.Errors);
        }
        catch
        {
            // We are swallowing the exception here, so the func delegate may want to have some logging.
            // TODO: We probably want to capture the exception or at least the context.
            // This would allow for dynamic error object creation, and access to the exception as part of the result.
            return Result<TOut>.Failure(error);
        }
    }
}
