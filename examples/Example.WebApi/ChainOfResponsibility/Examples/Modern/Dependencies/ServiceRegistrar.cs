using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Dependencies;
using Enterprise.DI.Core.Registration.Abstract;
using Example.WebApi.ChainOfResponsibility.Examples.Modern.Handlers;

namespace Example.WebApi.ChainOfResponsibility.Examples.Modern.Dependencies;

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
