using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public Result TryCatch(Func<Result> func)
    {
        try
        {
            return IsSuccess ? func() : Failure(Errors);
        }
        catch (Exception ex)
        {
            return ex.ToResult();
        }
    }

    public Result TryCatch(Action func, Error error)
    {
        try
        {
            if (IsSuccess)
            {
                func();
                return this;
            }

            return Failure(Errors);
        }
        catch
        {
            return Failure(error);
        }
    }

    public async Task<Result> TryCatchAsync(Func<Task<Result>> func)
    {
        try
        {
            return IsSuccess ? await func().ConfigureAwait(false) : Failure(Errors);
        }
        catch (Exception ex)
        {
            return ex.ToResult();
        }
    }

    public async Task<Result> TryCatchAsync(Func<Task> func, Error error)
    {
        try
        {
            if (IsSuccess)
            {
                await func().ConfigureAwait(false);
                return this;
            }

            return Failure(Errors);
        }
        catch
        {
            return Failure(error);
        }
    }
}
