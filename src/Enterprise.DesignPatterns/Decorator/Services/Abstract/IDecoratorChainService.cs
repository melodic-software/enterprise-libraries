namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorChainService
{
    IEnumerable<T> GetAllDecorators<T>(T current) where T : class;
    T? GetDecoratorAtIndex<T>(T current, int index) where T : class;
}