using Enterprise.Api.ErrorHandling.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Enterprise.Api.Validation.Filters;

// https://code-maze.com/aspnetcore-modelstate-validation-web-api/

public sealed class ModelStateValidationFilterAttribute : ActionFilterAttribute
{
    // TODO: Move to constants file.
    private const string DtoSuffix = "Dto";
    private const string ModelSuffix = "Model"; // TODO: Do we want to include this by default?

    private readonly List<string> _typeNameSuffixes = [];
    private readonly List<Type> _apiDataContractTypes = [];

    public ModelStateValidationFilterAttribute(List<string> typeNameSuffixes)
    {
        _typeNameSuffixes = typeNameSuffixes;
    }

    public ModelStateValidationFilterAttribute(List<Type> apiDataContractTypes)
    {
        _apiDataContractTypes = apiDataContractTypes;
    }

    public ModelStateValidationFilterAttribute(Type apiDataContractType)
    {
        _apiDataContractTypes = [apiDataContractType];
    }

    public ModelStateValidationFilterAttribute()
    {
        _typeNameSuffixes = [DtoSuffix];
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // This happens BEFORE the action executes (in synchronous context).

        ApplyFilter(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // This happens AFTER the action is executed (in synchronous context).
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // This only applies to async action methods.
        
        // Execute any code before the action executes.
        ApplyFilter(context);

        // If a result value has been set, we cannot call the next filter / delegate.
        if (context.Result != null)
        {
            return;
        }

        ActionExecutedContext result = await next();

        // Execute any code after the action executes.
    }

    private void ApplyFilter(ActionExecutingContext context)
    {
        object? action = context.RouteData.Values["action"];
        object? controller = context.RouteData.Values["controller"];

        IDictionary<string, object?> actionArguments = context.ActionArguments;

        var apiDataContractArguments = actionArguments
            .Where(x =>
            {
                Type? paramType = x.Value?.GetType();

                if (paramType == null)
                {
                    return false;
                }

                bool isApiDataContract = IsApiDataContract(paramType);

                return isApiDataContract;
            }).ToList();

        var apiDataContractValues = apiDataContractArguments.Select(x => x.Value).ToList();

        if (apiDataContractValues.Any(x => x is null))
        {
            // Normally the ApiController attribute would catch this and automatically return a 400 "bad request".

            // If the default validation handling is disabled (SuppressModelStateInvalidFilter = false)
            // OR nullable annotations are enabled in the project AND the model type has been deemed nullable (adding ? to variable declaration)
            // THEN this will also pass through and require manual validation.

            context.Result = new BadRequestResult();

            return;
        }

        

        if (context.ModelState.IsValid)
        {
            return;
        }

        // See comments above on validation handling. Under specific (default) circumstances, we don't have to check model state.
        // However, we've set "SuppressModelStateInvalidFilter" to false so a 422 to can be returned instead of the default 400 (which is less accurate).
        // OR if we want more control over how problem details results are generated.
        // Normally these would be returned as ValidationProblem().
        // TODO: Do we want to use 422 or 400 here?
        // In most cases this will be due to DTO validation which shouldn't deal with business rules.
        // Validation here should be simple data type / format validation.
        context.Result = context.Controller is ControllerBase @base
            ? ValidationProblemService.CreateValidationProblem(@base)
            : new BadRequestObjectResult(context.ModelState);

        //context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }

    private bool IsApiDataContract(Type paramType)
    {
        return _typeNameSuffixes.Any(s => paramType.Name.EndsWith(s, StringComparison.OrdinalIgnoreCase)) || 
               _apiDataContractTypes.Any(t => t.IsAssignableFrom(paramType));
    }
}
