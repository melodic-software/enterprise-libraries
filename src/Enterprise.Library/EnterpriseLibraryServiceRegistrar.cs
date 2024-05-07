using Enterprise.DI.Core.Registration;
using Enterprise.Library.Services;
using Enterprise.Library.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Library;

internal class EnterpriseLibraryServiceRegistrar : IRegisterServices
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
