using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Alternate;

public abstract class RegistrationOptionsBase<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// This sets the service lifetime for the command handler registration.
    /// The default is a transient service lifetime, and is recommended for most registrations.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Provider further configuration to the command handler registration.
    /// </summary>
    public Action<IServiceCollection, RegistrationContext<IHandleCommand<TCommand, TResponse>>>? PostConfigure { get; set; }
}
