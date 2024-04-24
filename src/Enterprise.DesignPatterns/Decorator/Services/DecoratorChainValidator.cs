using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratorChainValidator : IDecoratorChainValidator
{
    /// <inheritdoc />
    public bool IsDecoratorChainValid<T>(T current) where T : class
    {
        HashSet<T> visited = [];

        while (current is IDecorate<T> decorator)
        {
            if (!visited.Add(current))
                return false; // Circular reference detected

            current = decorator.Decorated;
        }

        return true;
    }
}