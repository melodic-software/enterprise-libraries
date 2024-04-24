using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.ErrorHandling.Options;

public class ErrorHandlingConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ErrorHandlingConfigOptions>(configuration, ErrorHandlingConfigOptions.ConfigSectionKey);
    }
}