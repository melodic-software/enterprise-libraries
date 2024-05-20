using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratorDepthService : IDecoratorDepthService
{
    /// <inheritdoc />
    public int GetDepthOfDecorator<T>(T current) where T : class
    {
        int depth = 0;
        T original = current;

        while (current is IDecorate<T> decorator)
        {
            depth++;
            current = decorator.Decorated;

            if (current.Equals(original))
            {
                return -1; // Indicates a circular reference.
            }
        }

        return depth;
    }
}
