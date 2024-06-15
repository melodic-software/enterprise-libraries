using Enterprise.Api.ErrorHandling.Domain;
using Enterprise.Api.ErrorHandling.Shared;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Api.Controllers.Abstract;

// TODO: We might need to create a secondary base class or apply this at individual controllers at the API instance level.

[ProducesResponseType(StatusCodes.Status401Unauthorized)] 
public class CustomControllerBase : ControllerBase
{
    public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
    {
        return ValidationProblemService.CreateValidationProblem(this);
    }

    [NonAction]
    public ActionResult ActionResultFrom(IEnumerable<IError> errors, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorActionResultFactory.CreateActionResult(errors.ToList(), this, problemDetailsFactory);
    }
}
