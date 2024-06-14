using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<T>
{
    public Result<TOut> TryCatch<TOut>(Func<T, Result<TOut>> func)
    {
        try
        {
            return IsSuccess ? func(Value) : Result<TOut>.Failure(Errors);
        }
        catch (Exception ex)
        {
            return ex.ToResult<TOut>();
        }
    }

    public Result<TOut> TryCatch<TOut>(Func<T, TOut> func, Error error)
    {
        try
        {
            return IsSuccess ? Result<TOut>.Success(func(Value)) : Result<TOut>.Failure(Errors);
        }
        catch
        {
            return Result<TOut>.Failure(error);
        }
    }

    public async Task<Result<TOut>> TryCatchAsync<TOut>(Func<T, Task<Result<TOut>>> func)
    {
        try
        {
            return IsSuccess ? await func(Value).ConfigureAwait(false) : Result<TOut>.Failure(Errors);
        }
        catch (Exception ex)
        {
            return ex.ToResult<TOut>();
        }
    }

    public async Task<Result<TOut>> TryCatchAsync<TOut>(Func<T, Task<TOut>> func, Error error)
    {
        try
        {
            return IsSuccess ? Result<TOut>.Success(await func(Value).ConfigureAwait(false)) : Result<TOut>.Failure(Errors);
        }
        catch
        {
            return Result<TOut>.Failure(error);
        }
    }
}
