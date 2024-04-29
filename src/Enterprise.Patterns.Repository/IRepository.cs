namespace Enterprise.Patterns.Repository;

public interface IRepository<T, in TId> : IReadOnlyRepository<T, TId>, IWriteRepository<T, TId>;
