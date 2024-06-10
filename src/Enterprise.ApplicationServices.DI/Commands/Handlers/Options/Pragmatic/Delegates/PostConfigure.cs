using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.DI.Core.Registration.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Pragmatic.Delegates;

public delegate void PostConfigure<TCommand, TResult>(IServiceCollection services, RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext)
    where TCommand : class, ICommand<TResult>;
