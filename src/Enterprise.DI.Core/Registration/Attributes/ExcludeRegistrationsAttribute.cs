using Enterprise.DI.Core.Registration.Abstract;

namespace Enterprise.DI.Core.Registration.Attributes;

/// <summary>
/// Decorate types with this attribute to ensure that any DI registrations are not automatically registered.
/// This is primarily concerned with classes that implement <see cref="IRegisterServices"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ExcludeRegistrationsAttribute : Attribute;
