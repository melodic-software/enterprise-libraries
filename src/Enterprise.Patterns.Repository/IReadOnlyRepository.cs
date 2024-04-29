namespace Enterprise.Patterns.Repository;

public interface IReadOnlyRepository<out T, in TId>
{
    T Get(TId id);
    IEnumerable<T> GetAll();
}
