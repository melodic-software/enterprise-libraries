using Enterprise.Api.Startup.Options;
using Enterprise.Api.Startup.Options.Abstract;

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
