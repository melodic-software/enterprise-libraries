using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Enterprise.Domain.Validation;

/// <summary>
/// Provides guard clause methods for validating strings.
/// These methods throw exceptions when validation fails, preventing further execution.
/// </summary>
public static class Ensure
{
    /// <summary>
    /// Validates that a given string is not null or empty.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the parameter being validated, captured automatically.
    /// This is used in the exception message to identify the failing parameter.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input string is null or empty.
    /// </exception>
    public static void NotNullOrEmpty([NotNull] string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = default)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(paramName));
    }

    /// <summary>
    /// Validates that a given string is not null, empty, or composed only of white-space characters.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the parameter being validated, captured automatically.
    /// This is used in the exception message to identify the failing parameter.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input string is null, empty, or white-space.
    /// </exception>
    public static void NotNullOrWhiteSpace([NotNull] string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, paramName ?? nameof(value));
    }
}