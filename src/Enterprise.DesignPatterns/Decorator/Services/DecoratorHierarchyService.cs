using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

internal class DecoratorHierarchyService : IDecoratorHierarchyService
{
    /// <inheritdoc />
    public T? GetParentDecorator<T>(T current) where T : class
    {
        T? previous = null;

        while (current is IDecorate<T> decorator)
        {
            previous = current;
            current = decorator.Decorated;
        }

        return previous;
    }

    /// <inheritdoc />
    public T GetChildDecorator<T>(T current) where T : class
    {
        throw new NotImplementedException();
    }
}