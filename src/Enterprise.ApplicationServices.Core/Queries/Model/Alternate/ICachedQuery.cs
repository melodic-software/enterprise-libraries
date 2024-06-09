namespace Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

public interface ICachedQuery<out TResponse> : IQuery<TResponse>, ICachedQuery;
