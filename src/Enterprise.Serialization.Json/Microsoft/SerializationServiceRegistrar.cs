using System.Text.Json;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Serialization.Json.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Serialization.Json.Microsoft;

public class SerializationServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISerializeJson>(_ =>
        {
            JsonSerializerOptions jsonSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();
            return new DefaultJsonSerializer(jsonSerializerOptions);
        });
    }
}
