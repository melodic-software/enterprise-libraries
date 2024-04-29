using System.Runtime.CompilerServices;

namespace Enterprise.Validation.Extensions;

public static class ArgumentExtensions
{
    /// <summary>
    /// Ensure that the provided value is not null.
    /// This is essentially a guard clause.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T NotNull<T>(this T value, [CallerArgumentExpression("value")] string? name = null)
    {
        // The caller argument expression will provide the name of what is being checked.
        ArgumentNullException.ThrowIfNull(value, name);

        return value;
    }
}
