using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<T>
{
    public Result<TOut> TryCatch<TOut>(Func<T, TOut> func)
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
            return IsSuccess ? func(Value) : Result<TOut>.Failure(Errors);
        }
        catch
        {
            // The exception is swallowed here.
            // The other method should be considered if access to exception details is required.
            // The delegate should probably contain logging code as a minimum.
            return error;
        }
    }

    public async Task<Result<TOut>> TryCatchAsync<TOut>(Func<T, Task<TOut>> func)
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
            return IsSuccess ? await func(Value).ConfigureAwait(false) : Result<TOut>.Failure(Errors);
        }
        catch
        {
            // The exception is swallowed here.
            // The other method should be considered if access to exception details is required.
            // The delegate should probably contain logging code as a minimum.
            return error;
        }
    }
}
