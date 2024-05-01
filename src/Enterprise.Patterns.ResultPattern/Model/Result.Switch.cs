using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    public void Switch(Action<T> onSuccess, Action<IEnumerable<IError>> onError)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
            return;
        }

        onError(Errors);
    }

    public async Task SwitchAsync(Func<T, Task> onSuccess, Func<IEnumerable<IError>, Task> onError)
    {
        if (IsSuccess)
        {
            await onSuccess(Value).ConfigureAwait(false);
            return;
        }

        await onError(Errors).ConfigureAwait(false);
    }

    public void SwitchFirst(Action<T> onSuccess, Action<IError> onFirstError)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
            return;
        }
        
        onFirstError(FirstError);
    }

    public async Task SwitchFirstAsync(Func<T, Task> onSuccess, Func<IError, Task> onFirstError)
    {
        if (IsSuccess)
        {
            await onSuccess(Value).ConfigureAwait(false);
            return;
        }

        await onFirstError(FirstError).ConfigureAwait(false);
    }
}
