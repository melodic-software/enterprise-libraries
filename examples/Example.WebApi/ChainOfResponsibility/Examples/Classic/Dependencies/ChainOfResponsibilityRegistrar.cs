using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Dependencies;
using Enterprise.DI.Core.Registration;
using Example.WebApi.ChainOfResponsibility.Examples.Classic.Handlers;

namespace Example.WebApi.ChainOfResponsibility.Examples.Classic.Dependencies;

public class ChainOfResponsibilityRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterClassicChainOfResponsibility<Document>()
            .WithSuccessor<DocumentLastModifiedHandler>()
            .WithSuccessor<DocumentApprovedByLitigationHandler>()
            .WithSuccessor<DocumentApprovedByManagementHandler>();
    }
}
