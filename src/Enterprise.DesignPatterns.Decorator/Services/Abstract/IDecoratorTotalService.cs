namespace Enterprise.DesignPatterns.Decorator.Services.Abstract;

public interface IDecoratorTotalService
{
    int GetTotalDecorations<T>(T current) where T : class;
}