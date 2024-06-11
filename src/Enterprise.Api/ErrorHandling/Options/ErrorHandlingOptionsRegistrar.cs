using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.ErrorHandling.Options;

internal sealed class ErrorHandlingOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ErrorHandlingOptions>(configuration, ErrorHandlingOptions.ConfigSectionKey);
    }
}
