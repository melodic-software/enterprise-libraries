namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorChainValidator
{
    bool IsDecoratorChainValid<T>(T current) where T : class;
}