using Enterprise.Api.Controllers.Formatters.Input;
using Enterprise.Api.Controllers.Formatters.Output;
using Enterprise.Api.Controllers.Options;
using Enterprise.Serialization.Json.Microsoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enterprise.Api.Controllers.Formatters;

public static class FormatterConfigService
{
    public static IMvcBuilder ConfigureFormatters(this IMvcBuilder builder, FormatterOptions formatterOptions)
    {
        AddXmlInputOutputFormatters(builder);

        ConfigureInputFormatters(builder, formatterOptions.InputFormatters);
        ConfigureOutputFormatters(builder, formatterOptions.OutputFormatters);
        Configure(builder);

        FormatterPrinter.PrintInputFormatters(builder);
        FormatterPrinter.PrintOutputFormatters(builder);

        return builder;
    }

    private static void AddXmlInputOutputFormatters(IMvcBuilder builder)
    {
        // Removing this serializer can cause issues if SuppressModelStateInvalidFilter is set to true
        // AND your endpoints accept application/json and application/xml.
        // Serialization errors that occur (particularly XML) can be obfuscated and still resolve as an empty model in your action methods.
        builder.AddXmlSerializerFormatters();
       
        // The data contract serializer supports types like DateTimeOffset.
        // The regular XML serializer requires that a type is designed in a specific way in order to completely serialize.
        // It requires a default public constructor, public read/write members, etc.
        builder.AddXmlDataContractSerializerFormatters();
    }

    private static void ConfigureInputFormatters(IMvcBuilder builder, List<IInputFormatter> inputFormatters)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<MvcOptions>, InputFormatterConfigurer>(
                provider => new InputFormatterConfigurer(inputFormatters))
        );
    }

    private static void ConfigureOutputFormatters(IMvcBuilder builder, List<IOutputFormatter> outputFormatters)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<MvcOptions>, OutputFormatterConfigurer>(
                provider => new OutputFormatterConfigurer(outputFormatters)
            ));
    }

    /// <summary>
    /// This configures System.Text.Json-based formatters.
    /// </summary>
    /// <param name="builder"></param>
    private static void Configure(IMvcBuilder builder)
    {
        // This just calls builder.Services.Configure() internally.
        builder.AddJsonOptions(ConfigureJsonOptions);
    }

    /// <summary>
    /// These JSON options are under the Microsoft.AspNetCore.Mvc namespace.
    /// </summary>
    /// <param name="options"></param>
    private static void ConfigureJsonOptions(JsonOptions options)
    {
        JsonSerializerOptions serializerOptions = options.JsonSerializerOptions;

        JsonSerializerOptions defaultSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

        serializerOptions.PropertyNamingPolicy = defaultSerializerOptions.PropertyNamingPolicy;
        serializerOptions.DictionaryKeyPolicy = defaultSerializerOptions.DictionaryKeyPolicy;
        serializerOptions.PropertyNameCaseInsensitive = defaultSerializerOptions.PropertyNameCaseInsensitive;
        serializerOptions.ReferenceHandler = defaultSerializerOptions.ReferenceHandler;
        serializerOptions.WriteIndented = defaultSerializerOptions.WriteIndented;

        // This allows for all enums to be treated as string literals (including Swagger UI).
        serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
}
