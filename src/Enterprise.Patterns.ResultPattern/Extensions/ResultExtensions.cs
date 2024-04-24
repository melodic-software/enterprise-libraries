using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Extensions;

public static partial class ResultExtensions
{
    public static Result<T> ToResult<T>(T value) => value;
}