namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<TValue>
{
    public Result<TValue> Tap(Action<TValue> action)
    {
        if (IsSuccess)
        {
            action(Value);
        }

        return this;
    }

    public async Task<Result<TValue>> TapAsync(Func<TValue, Task> actionAsync)
    {
        if (IsSuccess)
        {
            await actionAsync(Value).ConfigureAwait(false);
        }

        return this;
    }
}
