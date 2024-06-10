using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<IEnumerable<IError>, TOut> onError)
    {
        return IsSuccess ? onSuccess(Value) : onError(Errors.ToList());
    }

    public async Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> onSuccess, Func<IEnumerable<IError>, Task<TOut>> onErrorAsync)
    {
        if (IsSuccess)
        {
            return await onSuccess(Value).ConfigureAwait(false);
        }

        return await onErrorAsync(Errors).ConfigureAwait(false);
    }

    public TOut MatchFirst<TOut>(Func<T, TOut> onSuccess, Func<IError, TOut> onFirstError)
    {
        return IsSuccess ? onSuccess(Value) : onFirstError(FirstError);
    }

    public async Task<TOut> MatchFirstAsync<TOut>(Func<T, Task<TOut>> onSuccess, Func<IError, Task<TOut>> onFirstErrorAsync)
    {
        if (IsSuccess)
        {
            return await onSuccess(Value).ConfigureAwait(false);
        }

        return await onFirstErrorAsync(FirstError).ConfigureAwait(false);
    }
}
