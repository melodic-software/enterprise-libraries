using Enterprise.Api.Options;
using Enterprise.Api.Startup;
using Example.WebApi.ChainOfResponsibility.Examples.Classic.Demo;
using Example.WebApi.ChainOfResponsibility.Examples.Modern.Demo;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Demo;

await WebApi.RunAsync(args, apiConfigOptions =>
{
    apiConfigOptions.Events.BuilderCreated += (builder) =>
    {
        return Task.CompletedTask;
    };

    apiConfigOptions.Events.RequestPipelineConfigured += async application =>
    {
        // These follow the more standard definition of chain of responsibility.
        ClassicDemo.Execute(application.Services);
        ModernDemo.Execute(application.Services);

        // This is a specific "pipeline" based implementation of the chain of responsibility pattern.
        // It is similar to a middleware request pipeline.
        await PipelineDemo.ExecuteAsync(application.Services);
    };

    apiConfigOptions.ConfigureControllers(options =>
    {
        options.EnableControllers = true;
    });
});
