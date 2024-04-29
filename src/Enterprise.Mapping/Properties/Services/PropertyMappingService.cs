using Enterprise.Mapping.Properties.Model;
using Enterprise.Mapping.Properties.Model.Abstract;
using Enterprise.Mapping.Properties.Services.Abstract;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.Mapping.Properties.Services;

public class PropertyMappingService : IPropertyMappingService
{
    private readonly IList<IPropertyMapping> _propertyMappings;
    private readonly PropertyNameService _propertyNameService;

    public PropertyMappingService(IList<IPropertyMapping> propertyMappings)
    {
        _propertyMappings = propertyMappings;
        _propertyNameService = new PropertyNameService();
    }

    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        // Get the matching mapping.
        List<PropertyMapping<TSource, TDestination>> matchingMapping = _propertyMappings
            .OfType<PropertyMapping<TSource, TDestination>>()
            .ToList();

        if (matchingMapping.Count == 1)
        {
            PropertyMapping<TSource, TDestination> first = matchingMapping.First();
            Dictionary<string, PropertyMappingValue> mappingDictionary = first.MappingDictionary;
            return mappingDictionary;
        }

        throw new($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>");
    }

    public bool MappingExistsFor<TSource, TDestination>(string propertyNames)
    {
        if (string.IsNullOrEmpty(propertyNames))
            return true;

        Dictionary<string, PropertyMappingValue> mappingDictionary = GetPropertyMapping<TSource, TDestination>();

        // The string is separated by commas, so we split it.
        string[] propertyNamesSplit = propertyNames.Split(Comma);

        // Trim the values.
        string[] trimmedPropertyNames = propertyNamesSplit.Select(s => s.Trim()).ToArray();

        // Run through the property names.
        bool allPropertiesExist = trimmedPropertyNames.All(trimmedPropertyName => PropertyExists(trimmedPropertyName, mappingDictionary));

        return allPropertiesExist;
    }

    protected virtual bool PropertyExists(string trimmedPropertyName, Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        string propertyName = _propertyNameService.GetPropertyName(trimmedPropertyName);

        // Find the matching property.
        if (!mappingDictionary.ContainsKey(propertyName))
            return false;

        return true;
    }
}
