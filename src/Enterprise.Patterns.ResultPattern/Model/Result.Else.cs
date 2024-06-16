using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public Result Else(Func<IEnumerable<IError>, IError> onError)
    {
        return IsSuccess ? this : onError(Errors).ToResult();
    }

    public Result Else(Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return IsSuccess ? this : onError(Errors).ToResult();
    }

    public Result Else(IError error)
    {
        return IsSuccess ? this : error.ToResult();
    }

    public Result Else(Func<IEnumerable<IError>, Result> onError)
    {
        return IsSuccess ? this : onError(Errors);
    }

    public Result Else(Result onError)
    {
        return IsSuccess ? this : onError;
    }

    public async Task<Result> ElseAsync(Func<IEnumerable<IError>, Task<Result>> onErrorAsync)
    {
        if (IsSuccess)
        {
            return this;
        }

        return await onErrorAsync(Errors).ConfigureAwait(false);
    }

    public async Task<Result> ElseAsync(Func<IEnumerable<IError>, Task<IError>> onErrorAsync)
    {
        return IsSuccess ? this : (await onErrorAsync(Errors).ConfigureAwait(false)).ToResult();
    }

    public async Task<Result> ElseAsync(Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onErrorAsync)
    {
        return IsSuccess ? this : (await onErrorAsync(Errors).ConfigureAwait(false)).ToResult();
    }

    public async Task<Result> ElseAsync(Task<IError> onErrorAsync)
    {
        return IsSuccess ? this : (await onErrorAsync.ConfigureAwait(false)).ToResult();
    }

    public async Task<Result> ElseAsync(Task<Result> onErrorAsync)
    {
        return IsSuccess ? this : await onErrorAsync.ConfigureAwait(false);
    }
}
