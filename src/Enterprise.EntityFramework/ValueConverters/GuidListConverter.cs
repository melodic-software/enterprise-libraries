using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static System.StringSplitOptions;

namespace Enterprise.EntityFramework.ValueConverters;

public class GuidListConverter : ValueConverter<List<Guid>, string>
{
    public GuidListConverter(ConverterMappingHints? mappingHints = null)
        : base(
            v => string.Join(',', v),
            v => v.Split(',', RemoveEmptyEntries).Select(Guid.Parse).ToList(),
            mappingHints)
    {
    }
}