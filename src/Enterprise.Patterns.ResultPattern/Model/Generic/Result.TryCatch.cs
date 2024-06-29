using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public Result<TOut> TryCatch<TOut>(Func<TValue, Result<TOut>> func)
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

    public Result<TOut> TryCatch<TOut>(Func<TValue, TOut> func, Error error)
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

    public async Task<Result<TOut>> TryCatchAsync<TOut>(Func<TValue, Task<Result<TOut>>> func)
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

    public async Task<Result<TOut>> TryCatchAsync<TOut>(Func<TValue, Task<TOut>> func, Error error)
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
