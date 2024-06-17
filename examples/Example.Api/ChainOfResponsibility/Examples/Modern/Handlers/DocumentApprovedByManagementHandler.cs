using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Modern.Handlers;

internal sealed class DocumentApprovedByManagementHandler : Handler<Document>
{
    public override bool CanHandle(Document request)
    {
        return !request.ApprovedByManagement;
    }

    public override void Handle(Document request)
    {
        List<string> memberNames = [nameof(request.ApprovedByManagement)];
        var validationResult = new ValidationResult("Document must be approved by management.", memberNames);
        throw new ValidationException(validationResult, null, null);
    }
}
