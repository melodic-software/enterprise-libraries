using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.ApplicationServices.Core.Commands.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand;
