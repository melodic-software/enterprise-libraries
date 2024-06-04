using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Alternate.Delegates;

public delegate void PostConfigure<TCommand, TResponse>(IServiceCollection services, RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext)
    where TCommand : ICommand<TResponse>;
