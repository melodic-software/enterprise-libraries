using System.ComponentModel.DataAnnotations;
using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Modern.Handlers;

internal sealed class DocumentTitleHandler : Handler<Document>
{
    public override bool CanHandle(Document request)
    {
        return string.IsNullOrWhiteSpace(request.Title);
    }

    public override void Handle(Document request)
    {
        List<string> memberNames = [nameof(request.Title)];
        var validationResult = new ValidationResult("Title must be filled out", memberNames);
        throw new ValidationException(validationResult, null, null);
    }
}
