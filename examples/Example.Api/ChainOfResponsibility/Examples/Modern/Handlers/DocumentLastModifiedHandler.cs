using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Modern.Handlers;

internal sealed class DocumentLastModifiedHandler : Handler<Document>
{
    public override bool CanHandle(Document request)
    {
        return request.LastModified < DateTime.UtcNow.AddDays(-30);
    }

    public override void Handle(Document request)
    {
        List<string> memberNames = [nameof(request.LastModified)];
        var validationResult = new ValidationResult("Document must be modified in the last 30 days.", memberNames);
        throw new ValidationException(validationResult, null, null);
    }
}
