using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> result, Func<TValue, Result<TOut>> onSuccess)
    {
        return (await result.ConfigureAwait(false)).Then(onSuccess);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> result, Func<TValue, TOut> onSuccess)
    {
        return (await result.ConfigureAwait(false)).Then(onSuccess);
    }

    public static async Task<Result<TValue>> ThenAsync<TValue>(this Task<Result<TValue>> result, Action<TValue> onSuccess)
    {
        return (await result.ConfigureAwait(false)).Then(onSuccess);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> result, Func<TValue, Task<Result<TOut>>> onSuccess)
    {
        return await (await result.ConfigureAwait(false)).ThenAsync(onSuccess).ConfigureAwait(false);
    }

    public static async Task<Result<TOut>> ThenAsync<TValue, TOut>(this Task<Result<TValue>> result, Func<TValue, Task<TOut>> onSuccess)
    {
        return await (await result.ConfigureAwait(false)).ThenAsync(onSuccess).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ThenAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> onSuccess)
    {
        return await (await result.ConfigureAwait(false)).ThenAsync(onSuccess).ConfigureAwait(false);
    }
}
