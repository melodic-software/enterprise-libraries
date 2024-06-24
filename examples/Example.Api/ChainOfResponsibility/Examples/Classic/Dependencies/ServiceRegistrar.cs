using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestOnly;
using Enterprise.DI.Core.Registration.Abstract;
using Example.Api.ChainOfResponsibility.Examples.Classic.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Classic.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterClassicChainOfResponsibility<Document>()
            .WithSuccessor<DocumentLastModifiedHandler>()
            .WithSuccessor<DocumentApprovedByLitigationHandler>()
            .WithSuccessor<DocumentApprovedByManagementHandler>();
    }
}
