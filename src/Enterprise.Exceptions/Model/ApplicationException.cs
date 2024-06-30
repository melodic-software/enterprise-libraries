using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model;
using static Enterprise.Exceptions.Messages.ExceptionMessages;

namespace Enterprise.Exceptions.Model;

/// <summary>
/// This should only be used whenever we run into a situation we do not know how to handle.
/// Otherwise, the <see cref="Result" /> should be used to return a failure itself.
/// In general, throwing exceptions in the code is not desired if we know how to handle that particular case.
/// </summary>
public sealed class MedleyException : Exception
{
    public string RequestName { get; }
    public Error? Error { get; }

    public MedleyException(string requestName, Exception? innerException = default, Error? error = null)
        : base(ApplicationExceptionMessage, innerException)
    {
        RequestName = requestName;
        Error = error ?? Error.None();
    }
}
