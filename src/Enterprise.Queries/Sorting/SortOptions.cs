namespace Enterprise.Queries.Sorting;

public class SortOptions
{
    public string? OrderBy { get; set; }

    public SortOptions(string? orderBy)
    {
        OrderBy = orderBy;

        // TODO: Split these into properties?
        // Absorb some of the logic in the property mapping / dynamic sort process?
        // We might as well give as much logic to the core "query" library.
    }
}