namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorTypeService
{
    IEnumerable<Type> GetDecoratorTypes<T>(T current) where T : class;
    bool IsTypeInChain<T>(T current, Type targetType) where T : class;
    T? FindSpecificType<T>(T current, Type targetType) where T : class;
}