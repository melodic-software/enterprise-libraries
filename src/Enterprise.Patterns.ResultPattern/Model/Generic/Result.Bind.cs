namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public Result<TOut> Bind<TOut>(Func<TValue, Result<TOut>> bind)
    {
        return IsSuccess ? bind(Value) : Result<TOut>.Failure(Errors);
    }

    public async Task<Result<TOut>> BindAsync<TOut>(Func<TValue, Task<Result<TOut>>> bindAsync)
    {
        if (IsSuccess)
        {
            return await bindAsync(Value).ConfigureAwait(false);
        }

        return Result<TOut>.Failure(Errors);
    }
}
