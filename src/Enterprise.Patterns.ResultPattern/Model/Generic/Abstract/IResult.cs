using Enterprise.Patterns.ResultPattern.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Generic.Abstract;

public interface IResult<out TValue> : IResult
{
    TValue? Value { get; }
}
