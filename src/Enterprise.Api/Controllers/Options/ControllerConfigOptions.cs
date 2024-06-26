﻿namespace Enterprise.Api.Controllers.Options;

public class ControllerConfigOptions
{
    public const string ConfigSectionKey = "Custom:Controllers";

    /// <summary>
    /// Toggles the registration of controller services.
    /// Consider disabling this if using minimal APIs.
    /// </summary>
    public bool EnableControllers { get; set; } = false;

    /// <summary>
    /// This essentially adds the [Authorize] attribute to all controllers.
    /// Defaults to true.
    /// </summary>
    public bool EnableGlobalAuthorizeFilter { get; set; } = true;

    /// <summary>
    /// If you have controllers in another project, you can add one or more type references here.
    /// Typically, this is a custom static class called "AssemblyReference" with no implementation.
    /// The type reference is used to get the assembly reference, which is added as an application part.
    /// </summary>
    public List<Type> ControllerAssemblyTypes { get; set; } = [];

    /// <summary>
    /// Options for configuring input and output formatters.
    /// </summary>
    public FormatterConfigOptions FormatterConfigOptions { get; set; } = new();
}