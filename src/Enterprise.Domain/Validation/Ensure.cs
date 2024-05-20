using System.Diagnostics.CodeAnalysis;

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
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input string is null or empty.
    /// </exception>
    public static void NotNullOrEmpty([NotNull] string? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
    }

    /// <summary>
    /// Validates that a given string is not null, empty, or composed only of white-space characters.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input string is null, empty, or white-space.
    /// </exception>
    public static void NotNullOrWhiteSpace([NotNull] string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
    }
}
