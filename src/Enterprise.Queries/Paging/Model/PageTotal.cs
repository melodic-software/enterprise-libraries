using Enterprise.Queries.Paging.Services;

namespace Enterprise.Queries.Paging.Model;

public class PageTotal
{
    public int Value { get; }

    public PageTotal(int totalCount, PageSize pageSize)
    {
        int totalPages = PagingCalculator.CalculateTotalPages(totalCount, pageSize);

        Value = totalPages;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}