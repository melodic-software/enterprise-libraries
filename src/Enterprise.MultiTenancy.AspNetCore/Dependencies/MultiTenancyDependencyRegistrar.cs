﻿using Enterprise.DI.Core.Registration;
using Enterprise.Multitenancy.Abstractions;
using Enterprise.Multitenancy.AspNetCore.Services;
using Enterprise.Multitenancy.Options;
using Enterprise.Options.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Multitenancy.AspNetCore.Dependencies;

public class MultiTenancyDependencyRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        MultiTenancyConfigOptions configOptions = services
            .RegisterOptions<MultiTenancyConfigOptions>(configuration, MultiTenancyConfigOptions.ConfigSectionKey);

        if (!configOptions.MultiTenancyEnabled)
            return;

        services.AddScoped(provider =>
        {
            MultiTenancyConfigOptions options = provider.GetRequiredService<IOptionsSnapshot<MultiTenancyConfigOptions>>().Value;

            IHttpContextAccessor httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            ILogger<TenantRequestHeaderService> logger = provider.GetRequiredService<ILogger<TenantRequestHeaderService>>();

            IGetTenantId tenantIdService = new TenantRequestHeaderService(httpContextAccessor, logger, options.TenantIdRequired);

            return tenantIdService;
        });
    }
}
