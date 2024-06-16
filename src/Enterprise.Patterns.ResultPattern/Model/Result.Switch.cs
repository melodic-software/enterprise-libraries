using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public void Switch(Action onSuccess, Action<IEnumerable<IError>> onError)
    {
        if (IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onError(Errors);
        }
    }

    public async Task SwitchAsync(Func<Task> onSuccessAsync, Func<IEnumerable<IError>, Task> onErrorAsync)
    {
        if (IsSuccess)
        {
            await onSuccessAsync().ConfigureAwait(false);
        }
        else
        {
            await onErrorAsync(Errors).ConfigureAwait(false);
        }
    }

    public void SwitchFirst(Action onSuccess, Action<IError> onFirstError)
    {
        if (IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onFirstError(FirstError);
        }
    }

    public async Task SwitchFirstAsync(Func<Task> onSuccessAsync, Func<IError, Task> onFirstErrorAsync)
    {
        if (IsSuccess)
        {
            await onSuccessAsync().ConfigureAwait(false);
        }
        else
        {
            await onFirstErrorAsync(FirstError).ConfigureAwait(false);
        }
    }
}
