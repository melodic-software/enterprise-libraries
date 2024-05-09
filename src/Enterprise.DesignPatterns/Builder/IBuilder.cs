namespace Enterprise.DesignPatterns.Builder;

public interface IBuilder<out T>
{
    T Build();
}
