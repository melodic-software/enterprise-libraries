using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        return result.Tap(action);
    }

    public static async Task<Result<TIn>> TapAsync<TIn>(this Task<Result<TIn>> resultTask, Action<TIn> action)
    {
        Result<TIn> result = await resultTask.ConfigureAwait(false);
        return result.Tap(action);
    }

    public static async Task<Result<TIn>> TapAsync<TIn>(this Result<TIn> result, Func<TIn, Task> actionAsync)
    {
        return await result.TapAsync(actionAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TIn>> TapAsync<TIn>(this Task<Result<TIn>> resultTask, Func<TIn, Task> actionAsync)
    {
        Result<TIn> result = await resultTask.ConfigureAwait(false);
        return await result.TapAsync(actionAsync).ConfigureAwait(false);
    }
}
