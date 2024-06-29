using Enterprise.Patterns.ResultPattern.Errors.Extensions;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public Result<TOut> Then<TOut>(Func<TValue, Result<TOut>> onSuccess)
    {
        return IsSuccess ? onSuccess(Value) : Errors.ToResult<TOut>();
    }

    public Result<TValue> Then(Action<TValue> onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
            return this;
        }

        return Errors.ToResult<TValue>();
    }

    public Result<TOut> Then<TOut>(Func<TValue, TOut> onSuccess)
    {
        return IsSuccess ? onSuccess(Value) : Errors.ToResult<TOut>();
    }

    public async Task<Result<TOut>> ThenAsync<TOut>(Func<TValue, Task<Result<TOut>>> onSuccessAsync)
    {
        if (IsSuccess)
        {
            return await onSuccessAsync(Value).ConfigureAwait(false);
        }

        return Errors.ToResult<TOut>();
    }

    public async Task<Result<TValue>> ThenAsync(Func<TValue, Task> onSuccessAsync)
    {
        if (IsSuccess)
        {
            await onSuccessAsync(Value).ConfigureAwait(false);
        }

        return Errors.ToResult<TValue>();
    }

    public async Task<Result<TOut>> ThenAsync<TOut>(Func<TValue, Task<TOut>> onSuccessAsync)
    {
        return IsSuccess ? await onSuccessAsync(Value).ConfigureAwait(false) : Errors.ToResult<TOut>();
    }
}
