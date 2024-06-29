using Enterprise.ApplicationServices.Core.UseCases;

namespace Enterprise.ApplicationServices.Core.Queries.Model.Base;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// This interface allows for referring to all variations of query objects.
/// </summary>
public interface IBaseQuery : IUseCase;
