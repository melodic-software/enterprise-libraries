namespace Enterprise.Queries.Paging.Delegates;

public delegate Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> query);

