using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

internal sealed class DecoratorHierarchyService : IDecoratorHierarchyService
{
    /// <inheritdoc />
    public T? GetChildDecorator<T>(T current) where T : class
    {
        if (current is not IDecorate<T> decorator)
        {
            return null;
        }

        T child = decorator.Decorated;

        if (child is IDecorate<T> childDecorator)
        {
            return childDecorator as T;
        }

        return null;
    }
}
