using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.Controllers.Formatters.Input;

public class InputFormatterConfigurer : IConfigureOptions<MvcOptions>
{
    private readonly List<IInputFormatter> _inputFormatters;

    public InputFormatterConfigurer() : this([])
    {
    }

    public InputFormatterConfigurer(List<IInputFormatter> inputFormatters)
    {
        _inputFormatters = inputFormatters;
    }

    public void Configure(MvcOptions options)
    {
        // This adds support for JSON patch using Newtonsoft.Json while leaving other input and output formatters unchanged.
        // In all other cases we want to be using the System.Text.Json library by Microsoft.
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch
        // https://datatracker.ietf.org/doc/html/rfc6902
        options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());

        // Add custom (application specific) input formatters.
        foreach (IInputFormatter inputFormatter in _inputFormatters)
        {
            options.InputFormatters.Add(inputFormatter);
        }
    }

    private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        // We have to use the NewtonsoftJson library to support PatchDocument conversion.
        // This is not supported by Microsoft's System.Text.Json formatters / serializer.

        // The official statement from Microsoft is that they are not going to replace it with System.Text.Json:
        // "The main reason is that this will require a huge investment from us, with not a very high value-add for the majority of our customers."

        // TODO: Creating a service provider like this in a real application is typically discouraged.

        ServiceProvider builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        NewtonsoftJsonPatchInputFormatter formatter = builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();

        return formatter;
    }
}
