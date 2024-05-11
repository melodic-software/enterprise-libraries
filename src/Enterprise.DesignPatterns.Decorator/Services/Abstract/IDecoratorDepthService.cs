namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorDepthService
{
    public int GetDepthOfDecorator<T>(T current) where T : class;
}