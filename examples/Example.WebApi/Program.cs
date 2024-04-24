using Enterprise.Api.Startup;

await WebApi.RunAsync(args, apiConfigOptions =>
{
    apiConfigOptions.Events.BuilderCreated += (builder) =>
    {
        return Task.CompletedTask;
    };
});