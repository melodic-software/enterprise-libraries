using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared;
using Example.WebApi.ChainOfResponsibility.Examples.Modern.Handlers;
using Microsoft.Extensions.Logging.Abstractions;

namespace Example.WebApi.ChainOfResponsibility.Examples.Modern.Demo;

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
        using var scope = services.CreateScope();

        // Typically you wouldn't need this, but we've registered two examples (classic and modern).
        // Normally you'd register one or the other for the specific request type.
        IResponsibilityChain<Document>? chainOfResponsibility = scope.ServiceProvider
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
