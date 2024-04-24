namespace Enterprise.Api.Client.Pagination;

public class PagingMetadataDto
{
    public int TotalCount { get; }
    public int PageSize { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }

    public PagingMetadataDto(int totalCount, int pageSize, int currentPage, int totalPages)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = totalPages;
    }
}