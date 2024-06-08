using Enterprise.Api.Startup;
using Enterprise.Api.Startup.Options;
using Example.WebApi.ChainOfResponsibility.Examples.Classic.Demo;
using Example.WebApi.ChainOfResponsibility.Examples.Modern.Demo;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Demo;

await WebApi.RunAsync(args, (options, events) =>
{
    events.BuilderCreated += (builder) =>
    {
        return Task.CompletedTask;
    };

    events.RequestPipelineConfigured += async application =>
    {
        // These follow the more standard definition of chain of responsibility.
        ClassicDemo.Execute(application.Services);
        ModernDemo.Execute(application.Services);

        // This is a specific "pipeline" based implementation of the chain of responsibility pattern.
        // It is similar to a middleware request pipeline.
        await PipelineDemo.ExecuteAsync(application.Services);
    };

    options.ConfigureControllers(controllerOptions =>
    {
        controllerOptions.EnableControllers = true;
    });
});
