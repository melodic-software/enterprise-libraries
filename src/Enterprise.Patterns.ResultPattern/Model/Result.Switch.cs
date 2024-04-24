using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{T}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    public void Switch(Action<T> onValue, Action<IEnumerable<IError>> onError)
    {
        if (IsFailure)
        {
            onError(Errors);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="Result{T}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed asynchronously.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onError">The asynchronous action to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchAsync(Func<T, Task> onValue, Func<IEnumerable<IError>, Task> onError)
    {
        if (IsFailure)
        {
            await onError(Errors).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{T}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onFirstError">The action to execute with the first error if the state is an error.</param>
    public void SwitchFirst(Action<T> onValue, Action<IError> onFirstError)
    {
        if (IsFailure)
        {
            onFirstError(FirstError);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="Result{T}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed asynchronously using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onFirstError">The asynchronous action to execute with the first error if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchFirstAsync(Func<T, Task> onValue, Func<IError, Task> onFirstError)
    {
        if (IsFailure)
        {
            await onFirstError(FirstError).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }
}