using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestOnly;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestOnly;
using Example.Api.ChainOfResponsibility.Examples.Classic.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Classic.Demo;

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
        IResponsibilityChain<Document>? chainOfResponsibility = scope.ServiceProvider
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
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
        }
    }
}
