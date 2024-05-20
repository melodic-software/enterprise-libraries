using Enterprise.Library.Core.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Enterprise.Api.Swagger.Services;

public static class ConflictingActionResolver
{
    /// <summary>
    /// This isn't a great solution.
    /// In most cases, using the [ApiExplorerSettings(IgnoreApi = true)] is a better workaround
    /// along with the use of operation filters.
    /// </summary>
    /// <param name="apiDescriptions"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static ApiDescription ResolveSimple(IEnumerable<ApiDescription> apiDescriptions, ILogger logger)
    {
        logger.LogInformation("Returning the first descriptor.");
        return apiDescriptions.First();
    }

    public static ApiDescription ResolveConflictingActions(IReadOnlyCollection<ApiDescription> apiDescriptions, ILogger logger, bool controllersEnabled)
    {
        logger.LogInformation("Resolving conflicting actions.");

        IReadOnlyCollection<ApiDescription> nonControllerDescriptors = apiDescriptions
            .Where(d => d.ActionDescriptor is not ControllerActionDescriptor)
            .ToList();

        IReadOnlyCollection<ApiDescription> controllerDescriptors = apiDescriptions
            .Where(d => d.ActionDescriptor is ControllerActionDescriptor)
            .ToList();

        bool isOnlyControllers = !nonControllerDescriptors.Any();
        bool isMixed = nonControllerDescriptors.Any() && controllerDescriptors.Any();

        if (controllersEnabled)
        {
            if (isMixed)
            {
                logger.LogInformation("Selecting first non-controller descriptor since controllers are enabled and there are mixed descriptors.");
                return GetFirst(nonControllerDescriptors, logger);
            }

            if (isOnlyControllers)
            {
                logger.LogInformation("Returning first controller descriptor since controllers are enabled.");
                return GetFirst(controllerDescriptors, logger);
            }
        }
        else
        {
            if (isMixed)
            {
                logger.LogInformation("Selecting first controller descriptor since controllers are disabled and there are mixed descriptors.");
                return GetFirst(controllerDescriptors, logger);
            }

            if (isOnlyControllers)
            {
                return GetFirst(apiDescriptions, logger);
            }

            logger.LogInformation("Returning first non-controller descriptor since controllers are disabled.");
            return GetFirst(nonControllerDescriptors, logger);
        }

        // Default to returning the first description if none of the above conditions are met
        return GetFirst(apiDescriptions, logger);
    }

    private static ApiDescription GetFirst(IReadOnlyCollection<ApiDescription> apiDescriptions, ILogger logger)
    {
        // For now this is a workaround for controller methods that have route restrictions based on media types.
        // [RequestHeaderMatchesMediaType] attribute (action/route constraint).

        ApiDescription firstDescription = apiDescriptions.First();
        var otherDescriptions = apiDescriptions.Skip(1).ToList();

        var otherSupportedResponseTypes = otherDescriptions
            .SelectMany(x => x.SupportedResponseTypes.Where(a => a.StatusCode == Status200OK))
            .ToList();

        if (!otherSupportedResponseTypes.Any())
        {
            return firstDescription;
        }

        logger.LogInformation(
            "Adding additional response types from the other descriptors." +
            " These are supported response types that return a 200 OK status code."
        );

        // This doesn't work because Swashbuckle internally uses a dictionary for status codes.
        // Having multiple for each status code will not work.
        firstDescription.SupportedResponseTypes.AddRange(otherSupportedResponseTypes);

        return firstDescription;
    }
}
