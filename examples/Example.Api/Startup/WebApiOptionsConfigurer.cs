using Enterprise.Api.Startup.Options;
using Enterprise.Api.Startup.Options.Abstract;

namespace Example.Api.Startup;

internal sealed class WebApiOptionsConfigurer : IConfigureWebApiOptions
{
    public static void Configure(WebApiOptions options)
    {
        options.ConfigureSignalR(signalROptions =>
        {
            signalROptions.SignalREnabled = true;
            signalROptions.MapHubs = builder =>
            {
                // https://www.youtube.com/watch?v=2i-HxCgIEuI
                // https://www.youtube.com/watch?v=f6NchpcCAho
                // https://www.youtube.com/watch?v=9_pRk7PwkpY
                // https://www.youtube.com/watch?v=O7oaxFgNuYo

                // https://app.pluralsight.com/ilx/video-courses/d263983b-5771-4d34-a895-06cee182c66b/9b0d2501-e37a-4bcc-9f4a-998e7cf713b6/58da268b-adf6-4f3a-9aca-5c8c9a8af901

                //builder.MapHub<>()
            };
        });

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
