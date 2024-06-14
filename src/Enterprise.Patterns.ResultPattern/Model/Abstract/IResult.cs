using Enterprise.Patterns.ResultPattern.Errors.Abstract;

namespace Enterprise.Patterns.ResultPattern.Model.Abstract;

public interface IResult
{
    List<IError> Errors { get; }
    bool IsSuccess { get; }
}
