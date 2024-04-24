using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, Result<TNextValue>> onValue)
    {
        return (await result.ConfigureAwait(false)).Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, TNextValue> onValue)
    {
        return (await result.ConfigureAwait(false)).Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> Then<TValue>(this Task<Result<TValue>> result, Action<TValue> action)
    {
        return (await result.ConfigureAwait(false)).Then(action);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, Task<Result<TNextValue>>> onValue)
    {
        return await (await result.ConfigureAwait(false)).ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, Task<TNextValue>> onValue)
    {
        return await (await result.ConfigureAwait(false)).ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided <paramref name="action"/> is executed asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ThenAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> action)
    {
        return await (await result.ConfigureAwait(false)).ThenAsync(action).ConfigureAwait(false);
    }
}