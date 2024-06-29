﻿using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

/// <summary>
/// These are extension methods that deal with error collections.
/// </summary>
public static class ErrorCollectionExtensions
{
    public static bool ContainsNotFound(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.NotFound));

    public static bool ContainsValidationErrors(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.Validation));

    public static bool ContainsBusinessRuleViolations(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.BusinessRule));

    public static bool ContainsConflict(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.Conflict));

    public static bool ContainsPermissionErrors(this IEnumerable<IError> errors) =>
        errors.Any(e => e.Descriptors.Any(ed => ed is ErrorDescriptor.Permission));

    public static List<IError> GetTrueErrors(this IEnumerable<IError> errors) =>
        errors.Where(e => e.IsTrueError()).ToList();

    public static bool HasTrueError(this IEnumerable<IError> errors)
    {
        var errorList = errors.ToList();
        bool hasTrueError = errorList.Any() && errorList.Any(e => e.IsTrueError());
        return hasTrueError;
    }

    public static Result ToResult(this IEnumerable<IError> errors) => Result.Failure(errors);
    public static Result<TValue> ToResult<TValue>(this IEnumerable<IError> errors) => Result<TValue>.Failure(errors);
}
