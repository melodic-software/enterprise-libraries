using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Domain.Events.Queuing.Options;

public class DomainEventQueuingConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<DomainEventQueuingConfigOptions>(configuration, DomainEventQueuingConfigOptions.ConfigSectionKey);
    }
}
