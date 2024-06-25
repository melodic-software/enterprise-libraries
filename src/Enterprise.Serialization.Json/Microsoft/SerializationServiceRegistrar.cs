using System.Text.Json;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Serialization.Json.Abstract;
using Enterprise.Serialization.Json.Abstract.Composite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Serialization.Json.Microsoft;

public class SerializationServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJsonSerializer>(_ =>
        {
            JsonSerializerOptions jsonSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();
            return new DefaultJsonSerializer(jsonSerializerOptions);
        });

        // These are more fine-grained interfaces to adhere to interface segregation principle (ISP).
        // Dependent classes can take on exactly the functionality they need to use.
        // The implementation is shared (implements the composite interface).
        services.AddSingleton<ISerializeJson>(provider => provider.GetRequiredService<IJsonSerializer>());
        services.AddSingleton<IDeserializeJson>(provider => provider.GetRequiredService<IJsonSerializer>());
    }
}
