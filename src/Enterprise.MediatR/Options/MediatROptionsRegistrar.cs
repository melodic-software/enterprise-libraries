using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Options;

internal sealed class MediatROptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<MediatROptions>(configuration, MediatROptions.ConfigSectionKey);
    }
}
