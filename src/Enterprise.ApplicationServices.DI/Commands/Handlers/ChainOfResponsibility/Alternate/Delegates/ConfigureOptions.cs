using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate.Delegates;

public delegate void ConfigureOptions<TCommand, TResponse>(RegistrationOptions<TCommand, TResponse> options)
    where TCommand : ICommand<TResponse>;
