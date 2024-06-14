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
        return (await resultTask.ConfigureAwait(false)).Bind(bind);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> bindAsync)
    {
        return await (await resultTask.ConfigureAwait(false)).BindAsync(bindAsync).ConfigureAwait(false);
    }
}
