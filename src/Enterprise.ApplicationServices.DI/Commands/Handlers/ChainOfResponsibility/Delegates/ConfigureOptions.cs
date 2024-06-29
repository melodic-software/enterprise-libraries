using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureOptions<TCommand>(RegistrationOptions<TCommand> options) where TCommand : class, ICommand;
