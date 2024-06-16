using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<T>
{
    public new Result<T> Else(Func<IEnumerable<IError>, IError> onError)
    {
        return IsSuccess ? Value : onError(Errors).ToResult<T>();
    }

    public new Result<T> Else(Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return IsSuccess ? Value : onError(Errors).ToResult<T>();
    }

    public new Result<T> Else(IError error)
    {
        return IsSuccess ? Value : error.ToResult<T>();
    }

    public Result<T> Else(Func<IEnumerable<IError>, T> onError)
    {
        return IsSuccess ? Value : onError(Errors);
    }

    public Result<T> Else(T onError)
    {
        return IsSuccess ? Value : onError;
    }

    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<T>> onErrorAsync)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onErrorAsync(Errors).ConfigureAwait(false);
    }

    public new async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<IError>> onErrorAsync)
    {
        return IsSuccess ? Value : (await onErrorAsync(Errors).ConfigureAwait(false)).ToResult<T>();
    }

    public new async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onErrorAsync)
    {
        return IsSuccess ? Value : (await onErrorAsync(Errors).ConfigureAwait(false)).ToResult<T>();
    }

    public new async Task<Result<T>> ElseAsync(Task<IError> onErrorAsync)
    {
        return IsSuccess ? Value : (await onErrorAsync.ConfigureAwait(false)).ToResult<T>();
    }

    public async Task<Result<T>> ElseAsync(Task<T> onErrorAsync)
    {
        return IsSuccess ? Value : await onErrorAsync.ConfigureAwait(false);
    }
}
