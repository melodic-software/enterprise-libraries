using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<TValue> ToResult<TValue>(TValue value) => value;
}
