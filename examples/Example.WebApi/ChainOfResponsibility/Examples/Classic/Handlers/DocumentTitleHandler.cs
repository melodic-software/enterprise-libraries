using System.ComponentModel.DataAnnotations;

namespace Example.WebApi.ChainOfResponsibility.Examples.Classic.Handlers;

internal sealed class DocumentTitleHandler : ClassicHandler<Document>
{
    public override void Handle(Document request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            List<string> memberNames = [nameof(request.Title)];
            var validationResult = new ValidationResult("Title must be filled out", memberNames);
            throw new ValidationException(validationResult, null, null);
        }

        Successor?.Handle(request);
    }
}
