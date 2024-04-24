using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Domain.Events.Exceptions;

public class DomainEventHandlingException : Exception
{
    public IError Error { get; }
    public List<IError> UnderlyingErrors { get; }

    public DomainEventHandlingException(IError error, List<IError>? underlyingErrors = null)
        : base(message: error.Message)
    {
        Error = error;
        UnderlyingErrors = underlyingErrors ?? [];
    }
}