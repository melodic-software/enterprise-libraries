﻿using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.ModularMonoliths.ModuleNaming;
using Enterprise.ModularMonoliths.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.ModularMonoliths.ModuleState;

internal sealed class ModuleStateServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IModuleStateService>(provider =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            IOptions<ModuleStateOptions> moduleStateOptions = provider.GetRequiredService<IOptions<ModuleStateOptions>>();
            IModuleNameService moduleNameService = provider.GetRequiredService<IModuleNameService>();
            return new ModuleStateService(configuration, moduleStateOptions, moduleNameService);
        });
    }
}
