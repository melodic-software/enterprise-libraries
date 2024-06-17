using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Example.Api.ChainOfResponsibility.Examples.Classic.Demo;
using Example.Api.ChainOfResponsibility.Examples.Modern.Demo;
using Example.Api.ChainOfResponsibility.Examples.Pipeline.Demo;

namespace Example.Api.Startup;

internal sealed class WebApiConfigEventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
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
    }
}
