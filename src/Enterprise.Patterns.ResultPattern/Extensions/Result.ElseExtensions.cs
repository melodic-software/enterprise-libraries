using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, TValue> onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, TValue onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, Task<TValue>> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Task<TValue> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, IError> onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return (await result.ConfigureAwait(false)).Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, IError error)
    {
        return (await result.ConfigureAwait(false)).Else(error);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, Task<IError>> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Task<IError> onError)
    {
        return await (await result.ConfigureAwait(false)).ElseAsync(onError).ConfigureAwait(false);
    }
}