using Enterprise.ApplicationServices.Core.Queries.Model.Base;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Options.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Options;

public abstract class RegistrationOptionsBase<TQuery, TResult> where TQuery : class, IBaseQuery
{
    /// <summary>
    /// This sets the service lifetime for the query handler registration.
    /// The default is a transient service lifetime, and is recommended for most registrations.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Provider further configuration to the query handler registration.
    /// </summary>
    public PostConfigure<TQuery, TResult>? PostConfigure { get; set; }
}
