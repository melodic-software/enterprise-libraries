using System.Reflection;

namespace Enterprise.Applications.DI.Registration.Methods.Validation;

internal static class RegistrationMethodValidationService
{
    internal static void Validate(RegistrationMethodConfig config)
    {
        MethodInfo? methodInfo = config.InterfaceType.GetMethod(config.MethodName, config.BindingFlags);

        if (methodInfo == null)
        {
            throw new InvalidOperationException($"{config.MethodName} method not found.");
        }

        ParameterInfo[] methodParameters = methodInfo.GetParameters();

        if (!config.ParameterInfosAreValid(methodParameters))
        {
            throw new InvalidOperationException($"{config.MethodName} parameters are invalid.");
        }
    }
}
