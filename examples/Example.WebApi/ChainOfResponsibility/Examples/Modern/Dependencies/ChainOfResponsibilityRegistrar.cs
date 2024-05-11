﻿using Enterprise.DI.Core.Registration;
using Example.WebApi.ChainOfResponsibility.Examples.Modern.Handlers;

namespace Example.WebApi.ChainOfResponsibility.Examples.Modern.Dependencies;

public class ChainOfResponsibilityRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterChainOfResponsibility<Document>()
            .WithSuccessor<DocumentLastModifiedHandler>()
            .WithSuccessor<DocumentApprovedByLitigationHandler>()
            .WithSuccessor<DocumentApprovedByManagementHandler>();
    }
}
