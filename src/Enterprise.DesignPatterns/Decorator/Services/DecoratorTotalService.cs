using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratorTotalService : IDecoratorTotalService
{
    /// <inheritdoc />
    public int GetTotalDecorations<T>(T current) where T : class
    {
        int count = 0;

        while (current is IDecorate<T> decorator)
        {
            count++;
            current = decorator.Decorated;
        }

        return count;
    }
}