using Enterprise.Api.Options;
using Enterprise.Api.Startup;

await WebApi.RunAsync(args, apiConfigOptions =>
{
    apiConfigOptions.Events.BuilderCreated += (builder) =>
    {
        return Task.CompletedTask;
    };

    apiConfigOptions.ConfigureControllers(options =>
    {
        options.EnableControllers = true;
    });
});
