using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task Switch<TValue>(this Task<Result<TValue>> result, Action<TValue> onValue, Action<IEnumerable<IError>> onError)
    {
        (await result.ConfigureAwait(false)).Switch(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task SwitchAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> onValue, Func<IEnumerable<IError>, Task> onError)
    {
        await (await result.ConfigureAwait(false)).SwitchAsync(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task SwitchFirst<TValue>(this Task<Result<TValue>> result, Action<TValue> onValue, Action<IError> onError)
    {
        (await result.ConfigureAwait(false)).SwitchFirst(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task SwitchFirstAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> onValue, Func<IError, Task> onError)
    {
        await (await result.ConfigureAwait(false)).SwitchFirstAsync(onValue, onError);
    }
}