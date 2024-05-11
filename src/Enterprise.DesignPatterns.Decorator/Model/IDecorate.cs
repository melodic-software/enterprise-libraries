namespace Enterprise.DesignPatterns.Decorator.Model;

/// <summary>
/// Implementations decorate another object instance.
/// The decorated object should be of the same type, but this is the non-generic version.
/// </summary>
public interface IDecorate
{
    /// <summary>
    /// Gets the decorated object.
    /// </summary>
    public object Decorated { get; }
}

/// <summary>
/// Implementations decorate another object instance.
/// This interface extends the non-generic IDecorate interface, providing type-safe access to the underlying decorated object.
/// </summary>
/// <typeparam name="T">The type of the decorated object.</typeparam>
public interface IDecorate<out T> : IDecorate
{
    /// <summary>
    /// Gets the decorated object with the specified generic type.
    /// This property hides the non-generic Decorated property from the base interface.
    /// </summary>
    public new T Decorated { get; }
}