using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Pragmatic.Delegates;

public delegate void PostConfigure<TCommand, TResult>(IServiceCollection services, RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext)
    where TCommand : class, ICommand<TResult>;
