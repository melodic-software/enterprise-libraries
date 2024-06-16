namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public Result Then(Func<Result> onSuccess)
    {
        return IsSuccess ? onSuccess() : Failure(Errors);
    }

    public Result Then(Action onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess();
        }

        return IsSuccess ? this : Failure(Errors);
    }

    public async Task<Result> ThenAsync(Func<Task<Result>> onSuccessAsync)
    {
        if (IsSuccess)
        {
            return await onSuccessAsync().ConfigureAwait(false);
        }

        return Failure(Errors);
    }

    public async Task<Result> ThenAsync(Func<Task> onSuccessAsync)
    {
        if (IsSuccess)
        {
            await onSuccessAsync().ConfigureAwait(false);
        }

        return IsSuccess ? this : Failure(Errors);
    }
}
