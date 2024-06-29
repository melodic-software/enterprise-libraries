using Enterprise.ApplicationServices.Core.Queries.Model.Base;

namespace Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface IQuery : IBaseQuery;
