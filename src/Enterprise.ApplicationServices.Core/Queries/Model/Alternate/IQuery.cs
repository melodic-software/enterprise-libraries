using Enterprise.Library.Core.Attributes;
using MediatR;

namespace Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// This interface allows for defining an explicit return type that is associated with the query.
/// </summary>
/// <typeparam name="TResult"></typeparam>
[AlternativeTo(typeof(IQuery))]
public interface IQuery<out TResult> : IQuery, IRequest<TResult>;
