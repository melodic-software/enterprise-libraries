using Enterprise.Applications.DI.Registration;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Enterprise.Api.Middleware.Custom;

internal static class AppMiddlewareRegistrar
{
    /// <summary>
    /// Automatically resolves instances of <see cref="IRegisterAppMiddleware"/> and invokes the registration method.
    /// This allows for automatic wiring up of application specific middleware in the request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public static void UseAppMiddleware(this WebApplication app)
    {
        RegistrationMethodInvocationService.InvokeUsing(
            new RegistrationMethodConfig(
                typeof(IRegisterAppMiddleware),
                nameof(IRegisterAppMiddleware.RegisterAppMiddleware),
                MethodParamsAreValid,
                [app]
            )
        );
    }

    private static bool MethodParamsAreValid(ParameterInfo[] methodParameters) =>
        methodParameters.Length == 1 &&
        methodParameters[0].ParameterType == typeof(WebApplication);
}