﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Testing.AspNetCore.Services;

public static class AuthenticatedUserContextService
{
    public const string UnitTestAuthenticationType = "UnitTest";

    public static HttpContext AssignAuthenticatedUser(List<Claim> userClaims, HttpContext? httpContext = null, Controller? controller = null)
    {
        var claimsIdentity = new ClaimsIdentity(userClaims, UnitTestAuthenticationType);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        httpContext ??= HttpContextCreationService.CreateDefaultContext();

        httpContext.User = claimsPrincipal;

        if (controller == null)
        {
            return httpContext;
        }

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return httpContext;
    }
}
