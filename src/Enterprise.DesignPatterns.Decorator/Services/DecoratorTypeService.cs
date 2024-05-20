using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratorTypeService : IDecoratorTypeService
{
    private readonly IDecoratorChainService _decoratorChainService;

    public DecoratorTypeService(IDecoratorChainService decoratorChainService)
    {
        _decoratorChainService = decoratorChainService;
    }

    /// <inheritdoc />
    public IEnumerable<Type> GetDecoratorTypes<T>(T current) where T : class
    {
        return _decoratorChainService.GetAllDecorators(current)
            .Select(decorator => decorator.GetType())
            .ToList();
    }

    /// <inheritdoc />
    public bool IsTypeInChain<T>(T current, Type targetType) where T : class
    {
        return FindSpecificType(current, targetType) != null;
    }

    /// <inheritdoc />
    public T? FindSpecificType<T>(T? current, Type targetType) where T : class
    {
        while (current != null)
        {
            if (current.GetType() == targetType)
            {
                return current;
            }

            if (current is IDecorate<T> decorator)
            {
                current = decorator.Decorated;
            }
            else
            {
                break;
            }
        }

        return null;
    }
}
