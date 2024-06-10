namespace Enterprise.Queries.Paging.Delegates;

public delegate Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> query);
public delegate TResult Map<in TSource, out TResult>(TSource source);
