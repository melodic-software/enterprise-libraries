using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.Messages.Core.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Model;

/// <summary>
/// This is a base marker interface.
/// It's intended to be used as a single top level reference for generic query constraints.
/// </summary>
public interface IBaseQuery : IMessage, IUseCase;
