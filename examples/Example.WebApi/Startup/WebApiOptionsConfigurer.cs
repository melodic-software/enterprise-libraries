using Enterprise.Api.Core.Startup.Options.Abstract;
using Enterprise.Api.Startup.Options;

namespace Example.WebApi.Startup;

internal sealed class WebApiOptionsConfigurer : IConfigureWebApiOptions
{
    public static void Configure(WebApiOptions options)
    {
        options.ConfigureControllers(controllerOptions =>
        {
            controllerOptions.EnableControllers = true;
        });
    }
}
