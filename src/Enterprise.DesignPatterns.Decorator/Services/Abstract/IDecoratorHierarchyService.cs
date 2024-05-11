namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorHierarchyService
{
    T? GetChildDecorator<T>(T current) where T : class;
}
