using Enterprise.Library.Core.Attributes;

namespace Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

[AlternativeTo(typeof(ICachedQuery))]
public interface ICachedQuery<out TResponse> : IQuery<TResponse>, ICachedQuery;
