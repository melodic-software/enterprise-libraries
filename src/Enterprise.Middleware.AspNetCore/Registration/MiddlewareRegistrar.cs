using System.Reflection;
using Enterprise.Applications.DI.Registration.Methods;
using Enterprise.Middleware.AspNetCore.Registration.Abstract;
using Microsoft.AspNetCore.Builder;

namespace Enterprise.Middleware.AspNetCore.Registration;

internal static class MiddlewareRegistrar
{
    /// <summary>
    /// Automatically resolves instances of <see cref="IRegisterMiddleware"/> and invokes the registration method.
    /// This allows for automatic wiring up of middleware at a specific point in the request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public static void UseMiddleware(this WebApplication app)
    {
        RegistrationMethodInvocationService.InvokeUsing(
            new RegistrationMethodConfig(
                typeof(IRegisterMiddleware),
                nameof(IRegisterMiddleware.RegisterMiddleware),
                MethodParamsAreValid,
                [app]
            )
        );
    }

    private static bool MethodParamsAreValid(ParameterInfo[] methodParameters) =>
        methodParameters.Length == 1 &&
        methodParameters[0].ParameterType == typeof(WebApplication);
}
