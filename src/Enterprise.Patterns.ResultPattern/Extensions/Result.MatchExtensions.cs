using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    public static async Task<TOut> MatchAsync<TValue, TOut>(this Task<Result<TValue>> result,
        Func<TValue, TOut> onSuccess, Func<IEnumerable<IError>, TOut> onError)
    {
        return (await result.ConfigureAwait(false)).Match(onSuccess, onError);
    }

    public static async Task<TOut> MatchAsync<TValue, TOut>(this Task<Result<TValue>> result,
        Func<TValue, Task<TOut>> onSuccess, Func<IEnumerable<IError>, Task<TOut>> onErrorAsync)
    {
        return await (await result.ConfigureAwait(false)).MatchAsync(onSuccess, onErrorAsync);
    }

    public static async Task<TOut> MatchFirstAsync<TValue, TOut>(this Task<Result<TValue>> result,
        Func<TValue, TOut> onSuccess, Func<IError, TOut> onError)
    {
        return (await result.ConfigureAwait(false)).MatchFirst(onSuccess, onError);
    }

    public static async Task<TOut> MatchFirstAsync<TValue, TOut>(this Task<Result<TValue>> result,
        Func<TValue, Task<TOut>> onSuccess, Func<IError, Task<TOut>> onErrorAsync)
    {
        return await (await result.ConfigureAwait(false)).MatchFirstAsync(onSuccess, onErrorAsync);
    }
}
