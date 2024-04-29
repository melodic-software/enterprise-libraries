using Microsoft.Extensions.Configuration;

namespace Enterprise.Options;

public static class ConfigurationBinder
{
    public static void Bind<TOptions>(TOptions currentValue, IConfigurationSection? configurationSection)
    {
        if (configurationSection == null)
            return;

        // This will overwrite any property values with those found in the config section.

        // WARNING: Collection based properties can result in duplicate values being added.
        // This was discovered with a List<string>. The fix was to change the type to a HashSet<string>.
        // This method was later added to accomodate for clearing collections.
        // TODO: We need to test this. Commenting out for now...
        //CollectionClearer.ClearCollections(_currentValue);

        configurationSection.Bind(currentValue);

        // We can use this as an alternative to deduplicate the collection, but it uses reflection.
        //EnumerableDeDuplicationService.DeduplicateIEnumerables(_currentValue);
    }
}
