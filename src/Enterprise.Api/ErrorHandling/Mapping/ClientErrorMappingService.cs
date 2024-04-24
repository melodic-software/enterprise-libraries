using Enterprise.Api.ErrorHandling.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Api.ErrorHandling.Mapping;

public static class ClientErrorMappingService
{
    public static void CustomizeClientErrorMapping(ApiBehaviorOptions options)
    {
        // We can customize the message and URL for specific status codes in error responses.
        options.ClientErrorMapping[StatusCodes.Status400BadRequest] = new ClientErrorData
        {
            Link = ClientErrorDataConstants.BadRequestLink,
            Title = ClientErrorDataConstants.BadRequestTitle
        };

        options.ClientErrorMapping[StatusCodes.Status422UnprocessableEntity] = new ClientErrorData
        {
            Link = ClientErrorDataConstants.UnprocessableEntityLink,
            Title = ClientErrorDataConstants.UnprocessableEntityTitle
        };

        // We can customize the message and URL for specific status codes in error responses.
        options.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData
        {
            Link = ClientErrorDataConstants.InternalServerErrorLink,
            Title = ClientErrorDataConstants.InternalServerErrorTitle
        };

        // Customize just the link (or title).
        options.ClientErrorMapping[StatusCodes.Status401Unauthorized].Link = ClientErrorDataConstants.UnauthorizedLink;
    }
}