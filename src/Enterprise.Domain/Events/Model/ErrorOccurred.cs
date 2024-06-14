using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Domain.Events.Model;

public sealed class ErrorOccurred : DomainEvent
{
    public IError Error { get; }

    public ErrorOccurred(IError error)
    {
        Error = error;
    }
}
