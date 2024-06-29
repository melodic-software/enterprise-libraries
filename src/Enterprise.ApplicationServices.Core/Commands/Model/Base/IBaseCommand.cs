using Enterprise.ApplicationServices.Core.UseCases;

namespace Enterprise.ApplicationServices.Core.Commands.Model.Base;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// This interface allows for referring to all variations of command objects.
/// </summary>
public interface IBaseCommand : IUseCase;
