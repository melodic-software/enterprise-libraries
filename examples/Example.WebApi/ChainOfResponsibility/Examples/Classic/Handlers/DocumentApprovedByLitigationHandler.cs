using System.ComponentModel.DataAnnotations;

namespace Example.WebApi.ChainOfResponsibility.Examples.Classic.Handlers;

internal sealed class DocumentApprovedByLitigationHandler : ClassicHandler<Document>
{
    public override void Handle(Document request)
    {
        if (!request.ApprovedByLitigation)
        {
            List<string> memberNames = [nameof(request.ApprovedByLitigation)];
            var validationResult = new ValidationResult("Document must be approved by litigation.", memberNames);
            throw new ValidationException(validationResult, null, null);
        }

        Successor?.Handle(request);
    }
}
