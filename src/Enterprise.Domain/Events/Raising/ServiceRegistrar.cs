using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.Domain.Events.Raising;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IRaiseDomainEvents>(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            return new DomainEventRaiser(eventRaiser);
        });
    }
}
