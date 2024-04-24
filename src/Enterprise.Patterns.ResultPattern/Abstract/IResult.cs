using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Patterns.ResultPattern.Abstract;

public interface IResult
{
    List<IError> Errors { get; }
    bool IsSuccess { get; }
}

public interface IResult<out TValue> : IResult
{
    TValue? Value { get; }
}