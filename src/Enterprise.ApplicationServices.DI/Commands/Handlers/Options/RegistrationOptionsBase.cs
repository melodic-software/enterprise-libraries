using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Options;

public abstract class RegistrationOptionsBase<TCommand> where TCommand : class, ICommand
{
    /// <summary>
    /// This sets the service lifetime for the command handler registration.
    /// The default is a transient service lifetime, and is recommended for most registrations.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Provider further configuration to the command handler registration.
    /// </summary>
    public PostConfigure<TCommand>? PostConfigure { get; set; }
}
