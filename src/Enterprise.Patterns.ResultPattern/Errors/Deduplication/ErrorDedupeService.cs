using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Errors.Deduplication;

/// <summary>
/// Provides services for deduplicating errors.
/// </summary>
public static class ErrorDedupeService
{
    /// <summary>
    /// Deduplicates a collection of errors based on their code, message, and descriptor count.
    /// </summary>
    /// <param name="errors">The collection of errors to deduplicate.</param>
    /// <returns>A collection of deduplicated errors.</returns>
    public static IEnumerable<IError> DedupeErrors(IEnumerable<IError> errors)
    {
        return errors
            .GroupBy(x => new { x.Code, x.Message, Count = x.Descriptors.Count() })
            .Select(x => x.First());
    }
}
