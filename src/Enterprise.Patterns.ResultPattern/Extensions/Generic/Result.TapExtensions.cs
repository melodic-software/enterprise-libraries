using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    /// <summary>
    /// Applies an action to the value of a successful result without changing the result.
    /// This is useful for performing side effects based on a successful result.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value.</typeparam>
    /// <param name="result">The initial result object.</param>
    /// <param name="action">The action to apply to the value if the initial result is successful. This action should not throw an exception.</param>
    /// <returns>The original result object.</returns>
    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }
}
