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

    // TODO: We might want to conditionally add middleware based on this.
    apiConfigOptions.ConfigureDomainEventQueuing(options =>
    {
        options.EnableDomainEventQueuing = true;
    });
});
