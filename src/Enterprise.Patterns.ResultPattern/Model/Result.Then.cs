using Enterprise.Patterns.ResultPattern.Errors.Extensions;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public Result<TNextValue> Then<TNextValue>(Func<T, Result<TNextValue>> onValue)
    {
        return IsFailure ? Errors.ToResult<TNextValue>() : onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    public Result<T> Then(Action<T> action)
    {
        if (IsFailure)
            return Errors.ToResult<T>();

        action(Value);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public Result<TNextValue> Then<TNextValue>(Func<T, TNextValue> onValue)
    {
        return IsFailure ? Errors.ToResult<TNextValue>() : onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<T, Task<Result<TNextValue>>> onValue)
    {
        if (IsFailure)
            return Errors.ToResult<TNextValue>();

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked asynchronously.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    public async Task<Result<T>> ThenAsync(Func<T, Task> action)
    {
        if (IsFailure)
            return Errors.ToResult<T>();

        await action(Value).ConfigureAwait(false);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<T, Task<TNextValue>> onValue)
    {
        return IsFailure ? Errors.ToResult<TNextValue>() : await onValue(Value).ConfigureAwait(false);
    }
}