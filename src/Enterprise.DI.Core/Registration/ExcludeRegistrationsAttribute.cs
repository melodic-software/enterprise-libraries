﻿namespace Enterprise.DI.Core.Registration;

/// <summary>
/// Decorate types with this attribute to ensure that any DI registrations are not automatically registered.
/// This is primarily concerned with classes that implement <see cref="IRegisterServices"/>.
/// </summary>
public class ExcludeRegistrationsAttribute : Attribute;