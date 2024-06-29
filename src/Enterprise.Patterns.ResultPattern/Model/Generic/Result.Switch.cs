﻿using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public void Switch(Action<TValue> onSuccess, Action<IEnumerable<IError>> onError)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
        }
        else
        {
            onError(Errors);
        }
    }

    public async Task SwitchAsync(Func<TValue, Task> onSuccess, Func<IEnumerable<IError>, Task> onErrorAsync)
    {
        if (IsSuccess)
        {
            await onSuccess(Value).ConfigureAwait(false);
        }
        else
        {
            await onErrorAsync(Errors).ConfigureAwait(false);
        }
    }

    public void SwitchFirst(Action<TValue> onSuccess, Action<IError> onFirstError)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
        }
        else
        {
            onFirstError(FirstError);
        }
    }

    public async Task SwitchFirstAsync(Func<TValue, Task> onSuccess, Func<IError, Task> onFirstErrorAsync)
    {
        if (IsSuccess)
        {
            await onSuccess(Value).ConfigureAwait(false);
        }
        else
        {
            await onFirstErrorAsync(FirstError).ConfigureAwait(false);
        }
    }
}
