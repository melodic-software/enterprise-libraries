using Enterprise.Api.Startup.Options;
using Enterprise.Api.Startup.Options.Abstract;

namespace Example.Api.Startup;

internal sealed class WebApiOptionsConfigurer : IConfigureWebApiOptions
{
    public static void Configure(WebApiOptions options)
    {
        options.ConfigureSwaggerUI(uiOptions =>
        {
            uiOptions.PostConfigureUI = o =>
            {
                o.InjectStylesheet("/swagger-ui/custom.css");
                o.InjectJavascript("/swagger-ui/custom.js");
            };
        });
    }
}
