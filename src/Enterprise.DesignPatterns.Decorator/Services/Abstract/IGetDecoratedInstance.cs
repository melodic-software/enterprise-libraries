using Enterprise.DesignPatterns.Decorator.Model;

namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IGetDecoratedInstance
{
    /// <summary>
    /// Retrieves the innermost decorated instance in a decorator chain.
    /// </summary>
    /// <param name="current">The starting point of the decorator chain.</param>
    /// <returns>The innermost decorated instance.</returns>
    T GetInnermost<T>(T current) where T : class;

    /// <summary>
    /// Retrieves the innermost decorated instance starting from a non-generic decorator.
    /// </summary>
    /// <param name="current">The starting point of the decorator chain as a non-generic decorator.</param>
    /// <returns>The innermost decorated instance if it exists; otherwise, null.</returns>
    T? GetInnermost<T>(IDecorate? current) where T : class;

    /// <summary>
    /// Retrieves the innermost decorated instance in a generic decorator chain.
    /// </summary>
    /// <param name="current">The starting point of the generic decorator chain.</param>
    /// <returns>The innermost decorated instance.</returns>
    T GetInnermost<T>(IDecorate<T> current) where T : class;

    /// <summary>
    /// Retrieves the innermost decorated instance in a decorator chain, starting from a non-generic object.
    /// </summary>
    /// <param name="current">The starting point of the decorator chain as an object.</param>
    /// <returns>The innermost decorated instance if it exists; otherwise, null.</returns>
    T? GetInnermost<T>(object current) where T : class;
}