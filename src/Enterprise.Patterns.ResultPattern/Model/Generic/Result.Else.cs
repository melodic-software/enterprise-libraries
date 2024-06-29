using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public new Result<TValue> Else(Func<IEnumerable<IError>, IError> onError)
    {
        return IsSuccess ? Value : onError(Errors).ToResult<TValue>();
    }

    public new Result<TValue> Else(Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return IsSuccess ? Value : onError(Errors).ToResult<TValue>();
    }

    public new Result<TValue> Else(IError error)
    {
        return IsSuccess ? Value : error.ToResult<TValue>();
    }

    public Result<TValue> Else(Func<IEnumerable<IError>, TValue> onError)
    {
        return IsSuccess ? Value : onError(Errors);
    }

    public Result<TValue> Else(TValue onError)
    {
        return IsSuccess ? Value : onError;
    }

    public async Task<Result<TValue>> ElseAsync(Func<IEnumerable<IError>, Task<TValue>> onErrorAsync)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onErrorAsync(Errors).ConfigureAwait(false);
    }

    public new async Task<Result<TValue>> ElseAsync(Func<IEnumerable<IError>, Task<IError>> onErrorAsync)
    {
        return IsSuccess ? Value : (await onErrorAsync(Errors).ConfigureAwait(false)).ToResult<TValue>();
    }

    public new async Task<Result<TValue>> ElseAsync(Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onErrorAsync)
    {
        return IsSuccess ? Value : (await onErrorAsync(Errors).ConfigureAwait(false)).ToResult<TValue>();
    }

    public new async Task<Result<TValue>> ElseAsync(Task<IError> onErrorAsync)
    {
        return IsSuccess ? Value : (await onErrorAsync.ConfigureAwait(false)).ToResult<TValue>();
    }

    public async Task<Result<TValue>> ElseAsync(Task<TValue> onErrorAsync)
    {
        return IsSuccess ? Value : await onErrorAsync.ConfigureAwait(false);
    }
}
