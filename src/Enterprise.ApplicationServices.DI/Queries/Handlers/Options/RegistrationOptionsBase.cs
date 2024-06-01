using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Options;

public abstract class RegistrationOptionsBase<TQuery, TResponse> where TQuery : IBaseQuery
{
    /// <summary>
    /// This sets the service lifetime for the query handler registration.
    /// The default is a transient service lifetime, and is recommended for most registrations.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Provider further configuration to the query handler registration.
    /// </summary>
    public Action<IServiceCollection, RegistrationContext<IHandleQuery<TQuery, TResponse>>>? PostConfigure { get; set; }
}
