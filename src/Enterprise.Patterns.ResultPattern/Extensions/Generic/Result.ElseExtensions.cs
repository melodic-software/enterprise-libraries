using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<TValue> Else<TValue>(this Result<TValue> result, Func<IEnumerable<IError>, TValue> onError)
    {
        return result.Else(onError);
    }

    public static Result<TValue> Else<TValue>(this Result<TValue> result, TValue onError)
    {
        return result.Else(onError);
    }

    public static Result<TValue> Else<TValue>(this Result<TValue> result, Func<IEnumerable<IError>, IError> onError)
    {
        return result.Else(onError);
    }

    public static Result<TValue> Else<TValue>(this Result<TValue> result, Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return result.Else(onError);
    }

    public static Result<TValue> Else<TValue>(this Result<TValue> result, IError error)
    {
        return result.Else(error);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Func<IEnumerable<IError>, TValue> onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, TValue onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Func<IEnumerable<IError>, Task<TValue>> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ElseAsync(onErrorAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Task<TValue> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ElseAsync(onErrorAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Func<IEnumerable<IError>, IError> onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, IError error)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return result.Else(error);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Func<IEnumerable<IError>, Task<IError>> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ElseAsync(onErrorAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ElseAsync(onErrorAsync).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> resultTask, Task<IError> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        return await result.ElseAsync(onErrorAsync).ConfigureAwait(false);
    }
}
