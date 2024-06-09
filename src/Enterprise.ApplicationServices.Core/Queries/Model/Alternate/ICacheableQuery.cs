using Enterprise.Library.Core.Attributes;

namespace Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

/// <summary>
/// A query that can be cached.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
[AlternativeTo(typeof(ICacheableQuery))]
public interface ICacheableQuery<out TResponse> : IQuery<TResponse>, ICacheableQuery;
