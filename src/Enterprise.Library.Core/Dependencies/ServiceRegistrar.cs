using Enterprise.DI.Core.Registration;
using Enterprise.Library.Core.Services;
using Enterprise.Library.Core.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Library.Core.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(provider =>
        {
            IByteArraySerializer byteArraySerializer = new ByteArraySerializer();

            return byteArraySerializer;
        });
    }
}
