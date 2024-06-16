namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public Result Tap(Action action)
    {
        if (IsSuccess)
        {
            action();
        }

        return this;
    }

    public async Task<Result> TapAsync(Func<Task> actionAsync)
    {
        if (IsSuccess)
        {
            await actionAsync().ConfigureAwait(false);
        }

        return this;
    }
}
