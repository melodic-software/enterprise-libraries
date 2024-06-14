using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static TOut Match<TValue, TOut>(this Result<TValue> result, Func<TValue, TOut> onSuccess, Func<IEnumerable<IError>, TOut> onError)
    {
        return result.Match(onSuccess, onError);
    }

    public static async Task<TOut> MatchAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, TOut> onSuccess, Func<IEnumerable<IError>, TOut> onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Match(onSuccess, onError);
    }

    public static async Task<TOut> MatchAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TOut>> onSuccessAsync, Func<IEnumerable<IError>, Task<TOut>> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.MatchAsync(onSuccessAsync, onErrorAsync).ConfigureAwait(false);
    }

    public static TOut MatchFirst<TValue, TOut>(this Result<TValue> result, Func<TValue, TOut> onSuccess, Func<IError, TOut> onError)
    {
        return result.MatchFirst(onSuccess, onError);
    }

    public static async Task<TOut> MatchFirstAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, TOut> onSuccess, Func<IError, TOut> onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.MatchFirst(onSuccess, onError);
    }

    public static async Task<TOut> MatchFirstAsync<TValue, TOut>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TOut>> onSuccessAsync, Func<IError, Task<TOut>> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.MatchFirstAsync(onSuccessAsync, onErrorAsync).ConfigureAwait(false);
    }
}
