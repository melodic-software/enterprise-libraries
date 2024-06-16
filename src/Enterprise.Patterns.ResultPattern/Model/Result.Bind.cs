namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result
{
    public Result Bind(Func<Result> bind)
    {
        return IsSuccess ? bind() : Failure(Errors);
    }

    public async Task<Result> BindAsync(Func<Task<Result>> bindAsync)
    {
        if (IsSuccess)
        {
            return await bindAsync().ConfigureAwait(false);
        }

        return Failure(Errors);
    }
}
