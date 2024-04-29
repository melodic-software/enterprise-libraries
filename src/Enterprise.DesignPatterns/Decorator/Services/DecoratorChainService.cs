using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratorChainService : IDecoratorChainService
{
    /// <inheritdoc />
    public IEnumerable<T> GetAllDecorators<T>(T current) where T : class
    {
        while (current is IDecorate<T> decorator)
        {
            yield return current;
            current = decorator.Decorated;
        }
    }

    /// <inheritdoc />
    public T? GetDecoratorAtIndex<T>(T current, int index) where T : class
    {
        int currentIndex = 0;

        while (current is IDecorate<T> decorator)
        {
            if (currentIndex == index)
                return current;

            currentIndex++;
            current = decorator.Decorated;
        }

        return null;
    }
}
