using Enterprise.Mapping.Properties.Model.Abstract;

namespace Enterprise.Mapping.Properties.Model;

public class PropertyMapping<TSource, TDestination> : IPropertyMapping
{
    public Type SourceType => typeof(TSource);
    public Type DestinationType => typeof(TDestination);

    public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

    public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
    }
}