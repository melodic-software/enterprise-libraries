using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.Messages.Core.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes, and can be used to refer to all implementations of commands.
/// </summary>
public interface IBaseCommand : IUseCase, IMessage;
