using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.Generic;
using Enterprise.DI.Core.Registration.Abstract;
using Example.Api.ChainOfResponsibility.Examples.Pipeline.Handlers;
using Example.Api.ChainOfResponsibility.Examples.Pipeline.Models;

namespace Example.Api.ChainOfResponsibility.Examples.Pipeline.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterChainOfResponsibility<MyRequest, MyResponse>()
            .WithSuccessor<RequestLoggingHandler<MyRequest, MyResponse>>()
            .WithSuccessor<RequestExceptionHandler<MyRequest, MyResponse>>()
            .WithSuccessor<FirstPipelineHandler>()
            .WithSuccessor<SecondPipelineHandler>();
    }
}
