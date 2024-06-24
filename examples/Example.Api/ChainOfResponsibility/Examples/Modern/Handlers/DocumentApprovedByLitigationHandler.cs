using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

namespace Example.Api.ChainOfResponsibility.Examples.Modern.Handlers;

internal sealed class DocumentApprovedByLitigationHandler : Handler<Document>
{
    public override bool CanHandle(Document request)
    {
        return !request.ApprovedByLitigation;
    }

    public override void Handle(Document request)
    {
        List<string> memberNames = [nameof(request.ApprovedByLitigation)];
        var validationResult = new ValidationResult("Document must be approved by litigation.", memberNames);
        throw new ValidationException(validationResult, null, null);
    }
}
