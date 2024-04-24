using Enterprise.Queries.Paging.Extensions;

namespace Enterprise.Queries.Paging;

public class PagedList<T> : List<T>
{
    public PaginationMetadata PaginationMetadata { get; }

    public PagedList(List<T> items, PagingOptions pagingOptions, int totalCount) 
        : this(items, new PaginationMetadata(totalCount, pagingOptions))
    {

    }

    public PagedList(List<T> items, PaginationMetadata paginationMetadata)
    {
        AddRange(items);
        PaginationMetadata = paginationMetadata;
    }

    public static PagedList<T> Empty(PagingOptions pagingOptions)
    {
        List<T> items = [];
        PagedList<T> emptyList = new(items, pagingOptions, totalCount: 0);
        return emptyList;
    }

    public static async Task<PagedList<TSource>> CreateAsync<TSource>(IQueryable<TSource> query, PagingOptions pagingOptions, 
        Func<IQueryable<TSource>, Task<List<TSource>>> toListAsync)
    {
        int totalCount = query.Count();

        query = query.ApplyPaging(pagingOptions);

        List<TSource> items = await toListAsync.Invoke(query);

        PagedList<TSource> result = Create(items, pagingOptions, totalCount);

        return result;
    }

    public static async Task<PagedList<TResult>> CreateAsync<TSource, TResult>(IQueryable<TSource> query,
        PagingOptions pagingOptions, Func<IQueryable<TSource>, Task<List<TSource>>> toListAsync,
        Func<TSource, TResult> map)
    {
        PagedList<TSource> pagedList = await CreateAsync(query, pagingOptions, toListAsync);
        PagedList<TResult> result = Map(pagedList, map);
        return result;
    }

    public static PagedList<TResult> Map<TSource, TResult>(PagedList<TSource> pagedList, Func<TSource, TResult> map)
    {
        List<TResult> mappedItems = pagedList.Select(map).ToList();
        PagedList<TResult> result = new(mappedItems, pagedList.PaginationMetadata);
        return result;
    }

    public static PagedList<TSource> Create<TSource>(List<TSource> items, PagingOptions pagingOptions, int totalCount)
    {
        PagedList<TSource> result = new(items, pagingOptions, totalCount);

        return result;
    }
}