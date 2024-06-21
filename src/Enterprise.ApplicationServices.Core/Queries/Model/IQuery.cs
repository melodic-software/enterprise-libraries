using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.Messaging.Core.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface IQuery : IUseCase, IMessage;
