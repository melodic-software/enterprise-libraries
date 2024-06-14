using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<TOut> Then<TValue, TOut>(this Result<TValue> result, Func<TValue, Result<TOut>> onSuccess)
    {
        return result.Then(onSuccess);
    }

    public static Result<TOut> Then<TValue, TOut>(this Result<TValue> result, Func<TValue, TOut> onSuccess)
    {
        return result.Then(onSuccess);
    }

    public static Result<TValue> Then<TValue>(this Result<TValue> result, Action<TValue> onSuccess)
    {
        return result.Then(onSuccess);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, Result<TOut>> onSuccess)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Then(onSuccess);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, TOut> onSuccess)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Then(onSuccess);
    }

    public static async Task<Result<TValue>> ThenAsync<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> onSuccess)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Then(onSuccess);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, Task<Result<TOut>>> onSuccessAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ThenAsync(onSuccessAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TOut>> onSuccessAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ThenAsync(onSuccessAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ThenAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> onSuccessAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ThenAsync(onSuccessAsync).ConfigureAwait(false);
    }
}
