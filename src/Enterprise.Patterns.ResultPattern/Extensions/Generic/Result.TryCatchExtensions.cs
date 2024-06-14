using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func, Error error)
    {
        return result.TryCatch(func, error);
    }

    public static async Task<Result<TOut>> TryCatchAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> func, Error error)
    {
        Result<TIn> result = await resultTask.ConfigureAwait(false);
        return result.TryCatch(func, error);
    }

    public static async Task<Result<TOut>> TryCatchAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<TOut>> func, Error error)
    {
        Result<TIn> result = await resultTask.ConfigureAwait(false);
        return await result.TryCatchAsync(func, error).ConfigureAwait(false);
    }
}
