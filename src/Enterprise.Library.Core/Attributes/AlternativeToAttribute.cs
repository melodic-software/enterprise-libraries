﻿namespace Enterprise.Library.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
public class AlternativeToAttribute : Attribute
{
    public Type PreferredType { get; }

    public AlternativeToAttribute(Type preferredType)
    {
        PreferredType = preferredType;
    }
}