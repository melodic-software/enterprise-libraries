using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Services;

public class DecoratorIterationService : IIDecoratorIterationService
{
    private readonly IDecoratorChainService _decoratorChainService;

    public DecoratorIterationService(IDecoratorChainService decoratorChainService)
    {
        _decoratorChainService = decoratorChainService;
    }

    /// <inheritdoc />
    public void ForEachDecorator<T>(T current, Action<T> action) where T : class
    {
        foreach (T decorator in _decoratorChainService.GetAllDecorators(current))
        {
            action(decorator);
        }
    }
}
