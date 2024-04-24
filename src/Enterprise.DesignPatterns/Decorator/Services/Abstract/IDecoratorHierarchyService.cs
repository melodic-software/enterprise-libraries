namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorHierarchyService
{
    T? GetParentDecorator<T>(T current) where T : class;
    T? GetChildDecorator<T>(T current) where T : class;
}