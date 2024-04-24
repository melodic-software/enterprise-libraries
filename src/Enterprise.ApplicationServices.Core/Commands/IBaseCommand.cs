using Enterprise.ApplicationServices.Core.UseCases;
using Enterprise.Messages.Core.Model;

namespace Enterprise.ApplicationServices.Core.Commands;

public interface IBaseCommand : IMessage, IUseCase;