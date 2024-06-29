using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;
using Enterprise.DI.Registration.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Delegates;

public delegate void PostConfigure<TCommand>(IServiceCollection services, RegistrationContext<IHandleCommand<TCommand>> registrationContext)
    where TCommand : class, ICommand;
