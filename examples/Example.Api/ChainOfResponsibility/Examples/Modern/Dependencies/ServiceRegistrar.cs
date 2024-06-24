using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains.RequestOnly;
using Enterprise.DI.Core.Registration.Abstract;
using Example.Api.ChainOfResponsibility.Examples.Modern.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Modern.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterChainOfResponsibility<Document>()
            .WithSuccessor<DocumentLastModifiedHandler>()
            .WithSuccessor<DocumentApprovedByLitigationHandler>()
            .WithSuccessor<DocumentApprovedByManagementHandler>();
    }
}
