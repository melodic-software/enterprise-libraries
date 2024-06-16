using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public TOut Match<TOut>(Func<TOut> onSuccess, Func<IEnumerable<IError>, TOut> onError)
    {
        return IsSuccess ? onSuccess() : onError(Errors);
    }

    public async Task<TOut> MatchAsync<TOut>(Func<Task<TOut>> onSuccessAsync, Func<IEnumerable<IError>, Task<TOut>> onErrorAsync)
    {
        return IsSuccess ? await onSuccessAsync().ConfigureAwait(false) : await onErrorAsync(Errors).ConfigureAwait(false);
    }

    public TOut MatchFirst<TOut>(Func<TOut> onSuccess, Func<IError, TOut> onFirstError)
    {
        return IsSuccess ? onSuccess() : onFirstError(FirstError);
    }

    public async Task<TOut> MatchFirstAsync<TOut>(Func<Task<TOut>> onSuccessAsync, Func<IError, Task<TOut>> onFirstErrorAsync)
    {
        return IsSuccess ? await onSuccessAsync().ConfigureAwait(false) : await onFirstErrorAsync(FirstError).ConfigureAwait(false);
    }
}
