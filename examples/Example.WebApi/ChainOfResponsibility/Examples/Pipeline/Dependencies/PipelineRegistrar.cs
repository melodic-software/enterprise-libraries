using Enterprise.DI.Core.Registration;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Handlers;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Models;

namespace Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Dependencies;

public class PipelineRegistrar : IRegisterServices
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
