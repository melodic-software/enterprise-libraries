using System.Text.Json;
using System.Text.Json.Serialization;
using Enterprise.Serialization.Json.Microsoft;
using Enterprise.Serialization.Json.Microsoft.JsonNamingPolicies;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Serialization;

public static class SerializationConfigService
{
    public static void ConfigureSerialization(this IServiceCollection services)
    {
        // This calls "services.Configure<JsonOptions>()" under the hood.
        services.ConfigureHttpJsonOptions(ConfigureJsonOptions);
    }

    /// <summary>
    /// These JSON options are under the Microsoft.AspNetCore.Http.Json namespace.
    /// </summary>
    /// <param name="options"></param>
    private static void ConfigureJsonOptions(JsonOptions options)
    {
        JsonSerializerOptions serializerOptions = options.SerializerOptions;

        JsonSerializerOptions defaultSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

        serializerOptions.PropertyNamingPolicy = defaultSerializerOptions.PropertyNamingPolicy;
        serializerOptions.DictionaryKeyPolicy = new CustomJsonCamelCaseNamingPolicy();
        serializerOptions.PropertyNameCaseInsensitive = defaultSerializerOptions.PropertyNameCaseInsensitive;
        serializerOptions.ReferenceHandler = defaultSerializerOptions.ReferenceHandler;
        serializerOptions.WriteIndented = defaultSerializerOptions.WriteIndented;

        // This allows for all enums to be treated as string literals (including Swagger UI).
        serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
}
