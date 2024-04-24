using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.DesignPatterns.Decorator.Model;

public abstract class DecoratorBase<T> : IDecorate<T> where T : class
{
    private readonly IGetDecoratedInstance _decoratorService;

    /// <inheritdoc />
    object IDecorate.Decorated => Decorated;

    /// <inheritdoc />
    public T Decorated { get; }

    protected T InnermostHandler => _decoratorService.GetInnermost(Decorated);

    protected DecoratorBase(T decorated, IGetDecoratedInstance decoratorService)
    {
        _decoratorService = decoratorService;
        Decorated = decorated ?? throw new ArgumentNullException(nameof(decorated), "Decorated instance cannot be null.");
    }
}