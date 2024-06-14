using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static void Switch<TValue>(this Result<TValue> result, Action<TValue> onSuccess, Action<IEnumerable<IError>> onError)
    {
        result.Switch(onSuccess, onError);
    }

    public static async Task SwitchAsync<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> onSuccess, Action<IEnumerable<IError>> onError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        result.Switch(onSuccess, onError);
    }

    public static async Task SwitchAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> onSuccess, Func<IEnumerable<IError>, Task> onErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        await result.SwitchAsync(onSuccess, onErrorAsync).ConfigureAwait(false);
    }

    public static void SwitchFirst<TValue>(this Result<TValue> result, Action<TValue> onSuccess, Action<IError> onFirstError)
    {
        result.SwitchFirst(onSuccess, onFirstError);
    }

    public static async Task SwitchFirstAsync<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> onSuccess, Action<IError> onFirstError)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        result.SwitchFirst(onSuccess, onFirstError);
    }

    public static async Task SwitchFirstAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> onSuccess, Func<IError, Task> onFirstErrorAsync)
    {
        Result<TValue> result = await resultTask.ConfigureAwait(false);
        await result.SwitchFirstAsync(onSuccess, onFirstErrorAsync).ConfigureAwait(false);
    }
}
