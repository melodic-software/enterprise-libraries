namespace Enterprise.Patterns.Repository;

public interface IWriteRepository<in T, in TId>
{
    void Insert(T entity);
    void Update(T entity);
    void Delete(TId id);
}
