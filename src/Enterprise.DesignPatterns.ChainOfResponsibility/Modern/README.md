# Modern

The Modern variation of the Chain of Responsibility pattern enhances flexibility and integration capabilities with modern software development practices. This approach is suitable for applications that require dynamic handler registration, use of dependency injection, and need to incorporate advanced functionalities like logging and error handling directly within the chain.

**Key Characteristics**:
- **Dependency Injection (DI) Support**: Leverages dependency injection for initializing handlers, making the chain more flexible and easier to manage across different parts of the application.
- **Dynamic Handler Execution**: Handlers can be added or removed at runtime, and the handling logic can be altered based on runtime conditions.
- **Integrated Logging and Monitoring**: Incorporates logging within each handler, allowing for detailed monitoring and debugging of the request handling process.

**DI Registration Example**:
Here's how you would typically register the chain and its handlers in a .NET Core dependency injection container:

```csharp
using Enterprise.DI.Core.Registration;
using Medley.Api.ChainOfResponsibility.Modern.Dependencies;

namespace Medley.Api.ChainOfResponsibility.Examples.Modern.Dependencies;

internal sealed ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterChainOfResponsibility<Document>()
            .WithSuccessor<DocumentLastModifiedHandler>()
            .WithSuccessor<DocumentApprovedByLitigationHandler>()
            .WithSuccessor<DocumentApprovedByManagementHandler>();
    }
}
```

**Implementation Example**:
This example demonstrates a modern implementation of the Chain of Responsibility pattern where handlers are set up to handle documents, with logging integrated into the process.

```csharp
using System.ComponentModel.DataAnnotations;
using Medley.Api.ChainOfResponsibility.Modern.Chains;
using Medley.Api.ChainOfResponsibility.Shared;
using Microsoft.Extensions.Logging.Abstractions;

namespace Medley.Api.ChainOfResponsibility.Examples.Modern;

public static class ModernDemo
{
    public static void Execute(IServiceProvider services)
    {
        var validDocument = new Document("How to Avoid Java Development", DateTimeOffset.UtcNow, true, true);
        var invalidDocument = new Document("How to Avoid Java Development", DateTimeOffset.UtcNow, false, true);

        // Implicit chaining like this is not supported.
        // It is only supported with classic handlers.
        // The intent is to force use through a root chain of responsibility orchestrator.

        //var documentHandlerChain = new DocumentTitleHandler();

        //documentHandlerChain
        //    .SetSuccessor(new DocumentLastModifiedHandler())
        //    .SetSuccessor(new DocumentApprovedByLitigationHandler())
        //    .SetSuccessor(new DocumentApprovedByManagementHandler());

        // Manually construct an orchestrator with the name of the pattern.
        var manualChainOfResponsibility = new ResponsibilityChain<Document>([
            new DocumentTitleHandler(),
            new DocumentLastModifiedHandler(),
            new DocumentApprovedByLitigationHandler(),
            new DocumentApprovedByManagementHandler(),
        ], NullLogger<ResponsibilityChain<Document>>.Instance);

        // Get a pre-configured orchestrator instance from the DI container.
        using IServiceScope scope = services.CreateScope();

        // Typically you wouldn't need this, but we've registered two examples (classic and modern).
        // Normally you'd register one or the other for the specific request type.
        IResponsibilityChain<Document> chainOfResponsibility = scope.ServiceProvider
            .GetServices<IResponsibilityChain<Document>>()
            .FirstOrDefault(x => x.GetType() == typeof(ResponsibilityChain<Document>));

        try
        {
            //documentHandlerChain.Handle(validDocument);
            manualChainOfResponsibility.Handle(validDocument);
            chainOfResponsibility?.Handle(validDocument);
            Console.WriteLine("Valid document is valid.");

            //documentHandlerChain.Handle(invalidDocument);
            manualChainOfResponsibility.Handle(invalidDocument);
            chainOfResponsibility?.Handle(invalidDocument);
            Console.WriteLine("Invalid document is valid.");
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e);
        }
    }
}
```