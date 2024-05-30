using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.ApplicationServices.Core.Queries.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface IQuery : IBaseQuery;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// This interface allows for defining an explicit return type that is associated with the query.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IQuery, IRequest<Result<TResponse>>;
