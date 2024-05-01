using Enterprise.Patterns.ResultPattern.Errors.Extensions;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    public Result<TOut> Then<TOut>(Func<T, Result<TOut>> onSuccess)
    {
        return IsSuccess ? onSuccess(Value) : Errors.ToResult<TOut>();
    }

    public Result<T> Then(Action<T> onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
            return this;
        }

        return Errors.ToResult<T>();
    }

    public Result<TOut> Then<TOut>(Func<T, TOut> onSuccess)
    {
        return IsSuccess ? onSuccess(Value) : Errors.ToResult<TOut>();
    }

    public async Task<Result<TOut>> ThenAsync<TOut>(Func<T, Task<Result<TOut>>> onSuccess)
    {
        if (IsSuccess)
            return await onSuccess(Value).ConfigureAwait(false);
        
        return Errors.ToResult<TOut>();
    }

    public async Task<Result<T>> ThenAsync(Func<T, Task> onSuccess)
    {
        if (IsSuccess)
            await onSuccess(Value).ConfigureAwait(false);
        
        return Errors.ToResult<T>();
    }

    public async Task<Result<TOut>> ThenAsync<TOut>(Func<T, Task<TOut>> onSuccess)
    {
        return IsSuccess ? await onSuccess(Value).ConfigureAwait(false) : Errors.ToResult<TOut>();
    }
}
