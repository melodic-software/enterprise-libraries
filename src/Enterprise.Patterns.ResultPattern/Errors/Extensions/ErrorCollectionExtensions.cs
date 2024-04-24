using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

/// <summary>
/// Provides extension methods for collections of IError.
/// </summary>
public static class ErrorCollectionExtensions
{
    public static bool ContainsNotFound(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.NotFound));

    public static bool ContainsValidationErrors(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.Validation));

    public static bool ContainsConflict(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.Conflict));

    public static bool ContainsBusinessRuleViolations(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.BusinessRule));

    public static List<IError> GetTrueErrors(this IEnumerable<IError> errors) =>
        errors.Where(e => e.IsTrueError()).ToList();

    public static bool HasTrueError(this IEnumerable<IError> errors)
    {
        List<IError> errorList = errors.ToList();
        bool hasTrueError = errorList.Any() && errorList.Any(e => e.IsTrueError());
        return hasTrueError;
    }

    public static Result ToResult(this IEnumerable<IError> errors) => Result.Failure(errors);

    public static Result<T> ToResult<T>(this IEnumerable<IError> errors) => Result.Failure<T>(errors);
}