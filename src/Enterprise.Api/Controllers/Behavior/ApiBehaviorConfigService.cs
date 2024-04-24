using Enterprise.Api.ErrorHandling.Mapping;
using Enterprise.Api.ErrorHandling.ModelState;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Controllers.Behavior;

public static class ApiBehaviorConfigService
{
    public static void ConfigureApiBehavior(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
        {
            // Here we are configuring how the [ApiController] attribute should behave.

            // NOTE: This suppresses the default model state validation that is implemented due to the existence of the ApiController attribute.
            // This requires controllers to check for null model/DTO objects.
            // If nullable context is enabled in the .csproj, specifying a nullable input model will allow null request bodies and not trigger this validation.
            // https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references#nullable-contexts
            // When set to true, this allows us to manually check and return a 422 instead of the default 400 (which is less accurate).
            // When set to false, model validations are automatically handled and returned as ValidationProblem().
            // TODO: Enable this by default, but make it configurable.
            options.SuppressModelStateInvalidFilter = true;

            // This is what will execute when the model state is invalid.
            options.InvalidModelStateResponseFactory = ModelStateErrorService.GetInvalidModelStateResponseFactory;

            ClientErrorMappingService.CustomizeClientErrorMapping(options);
        });
    }
}