using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public TOut Match<TOut>(Func<TValue, TOut> onSuccess, Func<IEnumerable<IError>, TOut> onError)
    {
        return IsSuccess ? onSuccess(Value) : onError([..Errors]);
    }

    public async Task<TOut> MatchAsync<TOut>(Func<TValue, Task<TOut>> onSuccessAsync, Func<IEnumerable<IError>, Task<TOut>> onErrorAsync)
    {
        return IsSuccess ? await onSuccessAsync(Value) : await onErrorAsync(Errors);
    }

    public TOut MatchFirst<TOut>(Func<TValue, TOut> onSuccess, Func<IError, TOut> onFirstError)
    {
        return IsSuccess ? onSuccess(Value) : onFirstError(FirstError);
    }

    public async Task<TOut> MatchFirstAsync<TOut>(Func<TValue, Task<TOut>> onSuccessAsync, Func<IError, Task<TOut>> onFirstErrorAsync)
    {
        return IsSuccess ? await onSuccessAsync(Value) : await onFirstErrorAsync(FirstError);
    }
}
