# Classic

The Classic variation of the Chain of Responsibility pattern follows the traditional design where each handler has a single responsibility 
and the request is passed along the chain until it is handled. This variation is straightforward and easy to implement.

**Key Characteristics**:
- **Direct Succession**: Each handler knows and directly calls its successor if it cannot handle the request.
- **Simple and Predictable**: The request travels through a fixed path, making the flow easy to understand and debug.
- **Unmodified Passing**: The request is passed through the chain without modification, preserving its original state across handlers.

**DI Registration Example**:
Here's how you would typically register the chain and its handlers in a .NET Core dependency injection container:

```csharp
using Enterprise.DI.Core.Registration;
using Medley.Api.ChainOfResponsibility.Classic.Dependencies;

namespace Medley.Api.ChainOfResponsibility.Examples.Classic.Dependencies;

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
```

**Implementation Example**:
This example demonstrates a modern implementation of the Chain of Responsibility pattern where handlers are set up to handle documents, with logging integrated into the process.

```csharp
using System.ComponentModel.DataAnnotations;
using Medley.Api.ChainOfResponsibility.Classic.Chains;
using Medley.Api.ChainOfResponsibility.Shared;

namespace Medley.Api.ChainOfResponsibility.Examples.Classic;

public static class ClassicDemo
{
    public static void Execute(IServiceProvider services)
    {
        var validDocument = new Document("How to Avoid Java Development", DateTimeOffset.UtcNow, true, true);
        var invalidDocument = new Document("How to Avoid Java Development", DateTimeOffset.UtcNow, false, true);

        // This is an implicit chain, which is manually constructed and defined.
        var documentHandlerChain = new DocumentTitleHandler();

        documentHandlerChain
            .SetSuccessor(new DocumentLastModifiedHandler())
            .SetSuccessor(new DocumentApprovedByLitigationHandler())
            .SetSuccessor(new DocumentApprovedByManagementHandler());

        // Manually construct an orchestrator with the name of the pattern.
        var manualChainOfResponsibility = new ClassicResponsibilityChain<Document>([
            new DocumentTitleHandler(),
            new DocumentLastModifiedHandler(),
            new DocumentApprovedByLitigationHandler(),
            new DocumentApprovedByManagementHandler(),
        ]);

        // Get a pre-configured orchestrator instance from the DI container.
        using IServiceScope scope = services.CreateScope();

        // Typically you wouldn't need this, but we've registered two examples (classic and modern).
        // Normally you'd register one or the other for the specific request type.
        IResponsibilityChain<Document> chainOfResponsibility = scope.ServiceProvider
            .GetServices<IResponsibilityChain<Document>>()
            .FirstOrDefault(x => x.GetType() == typeof(ClassicResponsibilityChain<Document>));

        try
        {
            documentHandlerChain.Handle(validDocument);
            manualChainOfResponsibility.Handle(validDocument);
            chainOfResponsibility?.Handle(validDocument);
            Console.WriteLine("Valid document is valid.");

            documentHandlerChain.Handle(invalidDocument);
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