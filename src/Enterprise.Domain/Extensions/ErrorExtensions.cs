using Enterprise.Domain.Events.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Domain.Extensions;

public static class ErrorExtensions
{
    /// <summary>
    /// Translate the error to a <see cref="ErrorOccurred"/> event.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static ErrorOccurred ToEvent(this IError error) => new(error);

    /// <summary>
    /// Translate the errors to <see cref="ErrorOccurred"/> events.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static IEnumerable<ErrorOccurred> ToEvents(this IEnumerable<IError> errors) => errors.Select(e => e.ToEvent());
}
