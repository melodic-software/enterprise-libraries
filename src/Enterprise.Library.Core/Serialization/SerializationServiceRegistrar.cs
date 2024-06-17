using System.Text.Json;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Library.Core.Serialization.Abstract;
using Enterprise.Serialization.Json.Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Library.Core.Serialization;

internal sealed class SerializationServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(_ =>
        {
            JsonSerializerOptions jsonSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();
            IByteArraySerializer byteArraySerializer = new ByteArraySerializer(jsonSerializerOptions);
            return byteArraySerializer;
        });
    }
}
