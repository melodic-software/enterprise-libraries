namespace Enterprise.Patterns.ResultPattern.Model.Generic;

public partial class Result<T>
{
    public Result<T> Tap(Action<T> action)
    {
        if (IsSuccess)
        {
            action(Value);
        }

        return this;
    }

    public async Task<Result<T>> TapAsync(Func<T, Task> actionAsync)
    {
        if (IsSuccess)
        {
            await actionAsync(Value).ConfigureAwait(false);
        }

        return this;
    }
}
