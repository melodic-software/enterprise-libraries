namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IIDecoratorIterationService
{
    void ForEachDecorator<T>(T current, Action<T> action) where T : class;
}