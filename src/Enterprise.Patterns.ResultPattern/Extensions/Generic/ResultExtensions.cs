using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Extensions.Generic;

public static partial class ResultExtensions
{
    public static Result<T> ToResult<T>(T value) => value;
}
