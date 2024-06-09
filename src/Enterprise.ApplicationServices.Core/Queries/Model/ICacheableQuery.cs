namespace Enterprise.ApplicationServices.Core.Queries.Model;

/// <summary>
/// A query that can be cached.
/// </summary>
public interface ICacheableQuery : IQuery
{
    /// <summary>
    /// The unique cache identifier associated with the query.
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// The total time to live duration.
    /// </summary>
    public TimeSpan? Expiration { get; }
}
