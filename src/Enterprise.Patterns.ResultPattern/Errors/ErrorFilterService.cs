using Enterprise.Patterns.ResultPattern.Errors.Abstract;

namespace Enterprise.Patterns.ResultPattern.Errors;

/// <summary>
/// Provides services for filtering errors.
/// </summary>
public static class ErrorFilterService
{
    /// <summary>
    /// Filters out invalid errors that do not contain a code, message, or any descriptors.
    /// An error is considered invalid if the code is the default, and does not contain a message or descriptors.
    /// </summary>
    /// <param name="errors">The collection of errors to filter.</param>
    /// <returns>A collection of valid errors.</returns>
    public static IEnumerable<IError> FilterInvalid(IEnumerable<IError> errors)
    {
        var filteredErrors = errors
            .Where(x =>
                x.Code is not (null or Error.DefaultCode) ||
                !string.IsNullOrWhiteSpace(x.Message) ||
                x.Descriptors.Any())
            .ToList();

        return filteredErrors;
    }
}
