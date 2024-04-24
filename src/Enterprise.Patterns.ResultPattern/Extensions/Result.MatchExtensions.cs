using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<Result<TValue>> result,
        Func<TValue, TNextValue> onValue, Func<IEnumerable<IError>, TNextValue> onError)
    {
        return (await result.ConfigureAwait(false)).Match(onValue, onError);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> MatchAsync<TValue, TNextValue>(this Task<Result<TValue>> result,
        Func<TValue, Task<TNextValue>> onValue, Func<IEnumerable<IError>, Task<TNextValue>> onError)
    {
        return await (await result.ConfigureAwait(false)).MatchAsync(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> MatchFirst<TValue, TNextValue>(this Task<Result<TValue>> result,
        Func<TValue, TNextValue> onValue, Func<IError, TNextValue> onError)
    {
        return (await result.ConfigureAwait(false)).MatchFirst(onValue, onError);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> MatchFirstAsync<TValue, TNextValue>(this Task<Result<TValue>> result,
        Func<TValue, Task<TNextValue>> onValue, Func<IError, Task<TNextValue>> onError)
    {
        return await (await result.ConfigureAwait(false)).MatchFirstAsync(onValue, onError);
    }
}