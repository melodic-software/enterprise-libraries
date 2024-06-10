using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DI.Core.Registration.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Delegates;

public delegate void PostConfigure<TCommand>(IServiceCollection services, RegistrationContext<IHandleCommand<TCommand>> registrationContext)
    where TCommand : class, ICommand;
