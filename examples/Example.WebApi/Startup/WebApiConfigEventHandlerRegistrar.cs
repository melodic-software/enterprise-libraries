using Enterprise.Api.Core.Startup.Events.Abstract;
using Enterprise.Api.Startup.Events;
using Example.WebApi.ChainOfResponsibility.Examples.Classic.Demo;
using Example.WebApi.ChainOfResponsibility.Examples.Modern.Demo;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Demo;

namespace Example.WebApi.Startup;

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
