using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

namespace Example.WebApi.ChainOfResponsibility.Examples.Classic.Handlers;

internal sealed class DocumentApprovedByManagementHandler : ClassicHandler<Document>
{
    public override void Handle(Document request)
    {
        if (!request.ApprovedByManagement)
        {
            List<string> memberNames = [nameof(request.ApprovedByManagement)];
            var validationResult = new ValidationResult("Document must be approved by management.", memberNames);
            throw new ValidationException(validationResult, null, null);
        }

        Successor?.Handle(request);
    }
}
