using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, TValue> onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, TValue onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, Task<TValue>> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Task<TValue> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, IError> onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, IError error)
    {
        return (await result.ConfigureAwait(false)).Else(error);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, Task<IError>> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Task<IError> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }
}
