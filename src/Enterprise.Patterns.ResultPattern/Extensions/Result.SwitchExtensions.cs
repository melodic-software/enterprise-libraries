using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    public static async Task SwitchAsync<TValue>(this Task<Result<TValue>> result, Action<TValue> onSuccess, Action<IEnumerable<IError>> onError)
    {
        (await result.ConfigureAwait(false)).Switch(onSuccess, onError);
    }

    public static async Task SwitchAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> onSuccess, Func<IEnumerable<IError>, Task> onErrorAsync)
    {
        await (await result.ConfigureAwait(false)).SwitchAsync(onSuccess, onErrorAsync);
    }

    public static async Task SwitchFirstAsync<TValue>(this Task<Result<TValue>> result, Action<TValue> onSuccess, Action<IError> onError)
    {
        (await result.ConfigureAwait(false)).SwitchFirst(onSuccess, onError);
    }

    public static async Task SwitchFirstAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> onSuccess, Func<IError, Task> onErrorAsync)
    {
        await (await result.ConfigureAwait(false)).SwitchFirstAsync(onSuccess, onErrorAsync);
    }
}
