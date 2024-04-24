using Enterprise.Hosting.AspNetCore.Options;
using Enterprise.Options.Core.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Hosting.AspNetCore.Configuration;

/// <summary>
/// ASP.NET Core applications are by default self-hosted.
/// If we want to host our applications on IIS, we need to configure an IIS integration that will eventually help us with the deployment to IIS.
/// </summary>
public static class IISConfigService
{
    public static void ConfigureIISIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        IISIntegrationOptions customOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<IISIntegrationOptions>(configuration, IISIntegrationOptions.ConfigSectionKey);

        if (!customOptions.EnableIISIntegration)
            return;

        services.Configure<IISOptions>(options =>
        {
            options.AutomaticAuthentication = customOptions.AutomaticAuthentication;
            options.AuthenticationDisplayName = customOptions.AuthenticationDisplayName;
            options.ForwardClientCertificate = customOptions.ForwardClientCertificate;
        });
    }
}