namespace Enterprise.Library.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
public class AlternativeToAttribute : Attribute
{
    public Type PreferredType { get; }

    public AlternativeToAttribute(Type preferredType)
    {
        PreferredType = preferredType;
    }
}