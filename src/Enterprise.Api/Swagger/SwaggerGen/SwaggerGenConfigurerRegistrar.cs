using Enterprise.Api.Controllers.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Api.Versioning.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.SwaggerGen;

public static class SwaggerGenConfigurerRegistrar
{
    public static Func<IServiceProvider, IConfigureOptions<SwaggerGenOptions>> RegisterSwaggerGenConfigurer(IServiceCollection services)
    {
        return serviceProvider =>
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            IOptions<SwaggerOptions> swaggerOptions = serviceProvider.GetRequiredService<IOptions<SwaggerOptions>>();
            IOptions<SwaggerSecurityOptions> swaggerSecurityOptions = serviceProvider.GetRequiredService<IOptions<SwaggerSecurityOptions>>();
            IOptions<ControllerOptions> controllerOptions = serviceProvider.GetRequiredService<IOptions<ControllerOptions>>();
            IOptions<VersioningOptions> versioningOptions = serviceProvider.GetRequiredService<IOptions<VersioningOptions>>();
            ILogger<SwaggerGenOptionsConfigurer> logger = serviceProvider.GetRequiredService<ILogger<SwaggerGenOptionsConfigurer>>();

            // this is our primary configurer for swagger generation instead of the setupAction that can be passed into services.AddSwaggerGen()
            // we can inject other services in here as needed, which is one advantage over calling .AddSwaggerGen on the IServiceCollection instance
            IConfigureOptions<SwaggerGenOptions> result = new SwaggerGenOptionsConfigurer(
                swaggerOptions,
                swaggerSecurityOptions,
                controllerOptions,
                versioningOptions,
                configuration,
                logger,
                serviceProvider,
                services
            );

            return result;
        };
    }
}
