using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> bind)
    {
        return result.Bind(bind);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> bindAsync)
    {
        return await result.BindAsync(bindAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Result<TOut>> bind)
    {
        Result<TIn> result = await resultTask.ConfigureAwait(false);
        return result.Bind(bind);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> bindAsync)
    {
        Result<TIn> result = await resultTask.ConfigureAwait(false);
        return await result.BindAsync(bindAsync).ConfigureAwait(false);
    }
}
