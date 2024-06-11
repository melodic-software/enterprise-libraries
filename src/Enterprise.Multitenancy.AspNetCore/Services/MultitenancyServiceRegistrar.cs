using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Multitenancy.Abstractions;
using Enterprise.Multitenancy.Options;
using Enterprise.Options.Registration.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Multitenancy.AspNetCore.Services;

internal sealed class MultitenancyServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        MultitenancyOptions options = services.RegisterOptions<MultitenancyOptions>(configuration, MultitenancyOptions.ConfigSectionKey);

        if (!options.MultitenancyEnabled)
        {
            return;
        }

        services.AddScoped(provider =>
        {
            MultitenancyOptions multitenancyOptions = provider.GetRequiredService<IOptionsSnapshot<MultitenancyOptions>>().Value;

            IHttpContextAccessor httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            ILogger<TenantRequestHeaderService> logger = provider.GetRequiredService<ILogger<TenantRequestHeaderService>>();

            bool tenantIdRequired = multitenancyOptions.TenantIdRequired;

            IGetTenantId tenantIdService = new TenantRequestHeaderService(httpContextAccessor, logger, tenantIdRequired);

            return tenantIdService;
        });
    }
}
