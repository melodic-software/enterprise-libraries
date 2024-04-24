using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Extensions;

namespace Enterprise.Patterns.ResultPattern.Model;

public partial class Result<T>
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<T> Else(Func<IEnumerable<IError>, IError> onError)
    {
        return !IsFailure ? Value : onError(Errors).ToResult<T>();
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<T> Else(Func<IEnumerable<IError>, IEnumerable<IError>> onError)
    {
        return !IsFailure ? Value : onError(Errors).ToResult<T>();
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public Result<T> Else(IError error)
    {
        return !IsFailure ? Value : error.ToResult<T>();
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<T> Else(Func<IEnumerable<IError>, T> onError)
    {
        return !IsFailure ? Value : onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The value to return if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<T> Else(T onError)
    {
        return !IsFailure ? Value : onError;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<T>> onError)
    {
        if (!IsFailure)
            return Value;

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<IError>> onError)
    {
        return !IsFailure ? Value : (await onError(Errors).ConfigureAwait(false)).ToResult<T>();
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<T>> ElseAsync(Func<IEnumerable<IError>, Task<IEnumerable<IError>>> onError)
    {
        return !IsFailure ? Value : (await onError(Errors).ConfigureAwait(false)).ToResult<T>();
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is awaited and returned.
    /// </summary>
    /// <param name="error">The error to return if the state is error.</param>
    /// <returns>The result from awaiting the given <paramref name="error"/>.</returns>
    public async Task<Result<T>> ElseAsync(Task<IError> error)
    {
        return !IsFailure ? Value : (await error.ConfigureAwait(false)).ToResult<T>();
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<T>> ElseAsync(Task<T> onError)
    {
        return !IsFailure ? Value : await onError.ConfigureAwait(false);
    }
}