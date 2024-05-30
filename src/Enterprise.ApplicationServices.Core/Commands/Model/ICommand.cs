using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.Messages.Core.Model;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.ApplicationServices.Core.Commands.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface ICommand : IUseCase, IMessage, ICommand<Result>, IRequest<Result>;
