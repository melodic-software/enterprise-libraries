using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Extensions;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    public Result<T> Else(Func<IEnumerable<IError>, IError> onError)
    {
        return IsSuccess ? Value : onError(Errors).ToResult<T>();
    }

    public Result<T> Else(Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return IsSuccess ? Value : onError(Errors).ToResult<T>();
    }

    public Result<T> Else(IError error)
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

    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<T>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<IError>> onError)
    {
        return IsSuccess ? Value : (await onError(Errors).ConfigureAwait(false)).ToResult<T>();
    }

    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onError)
    {
        return IsSuccess ? Value : (await onError(Errors).ConfigureAwait(false)).ToResult<T>();
    }

    public async Task<Result<T>> ElseAsync(Task<IError> error)
    {
        return IsSuccess ? Value : (await error.ConfigureAwait(false)).ToResult<T>();
    }

    public async Task<Result<T>> ElseAsync(Task<T> onError)
    {
        return IsSuccess ? Value : await onError.ConfigureAwait(false);
    }
}
