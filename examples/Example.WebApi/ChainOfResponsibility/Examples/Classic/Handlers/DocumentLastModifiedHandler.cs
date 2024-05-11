using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

namespace Example.WebApi.ChainOfResponsibility.Examples.Classic.Handlers;

internal sealed class DocumentLastModifiedHandler : ClassicHandler<Document>
{
    public override void Handle(Document request)
    {
        if (request.LastModified < DateTime.UtcNow.AddDays(-30))
        {
            List<string> memberNames = [nameof(request.LastModified)];
            var validationResult = new ValidationResult("Document must be modified in the last 30 days.", memberNames);
            throw new ValidationException(validationResult, null, null);
        }

        Successor?.Handle(request);
    }
}
