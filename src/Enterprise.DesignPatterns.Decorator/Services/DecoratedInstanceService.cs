using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratedInstanceService : IGetDecoratedInstance
{
    /// <inheritdoc />
    public T GetInnermost<T>(T current) where T : class
    {
        while (current is IDecorate<T> decorator)
            current = decorator.Decorated;
            
        return current;
    }

    /// <inheritdoc />
    public T? GetInnermost<T>(IDecorate? current) where T : class
    {
        while (current is IDecorate<T> decorator)
        {
            // Use "null check" pattern.
            if (decorator.Decorated is { } next)
                current = next as IDecorate;
            else
                break;
        }

        return current as T;
    }

    /// <inheritdoc />
    public T GetInnermost<T>(IDecorate<T> current) where T : class
    {
        while (current is { Decorated: IDecorate<T> nextDecorator })
            current = nextDecorator;

        return current.Decorated;
    }

    /// <inheritdoc />
    public T? GetInnermost<T>(object current) where T : class
    {
        if (current is IDecorate<T> decorator)
            return GetInnermost(decorator);
            
        return null;
    }
}