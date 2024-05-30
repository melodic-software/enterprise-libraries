using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.Messages.Core.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes, and encapsulates all query implementations.
/// </summary>
public interface IBaseQuery : IUseCase, IMessage;
