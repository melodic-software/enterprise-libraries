using AutoMapper;
using Enterprise.Api.Client.Pagination;
using Enterprise.Queries.Paging;

namespace Enterprise.AutoMapper.Profiles;

public sealed class ApiClientProfile : Profile
{
    public ApiClientProfile()
    {
        CreateMap<PaginationMetadata, PagingMetadataDto>()
            .ConstructUsing(x => new PagingMetadataDto(x.TotalCount, x.PageSize.Value, x.CurrentPage.Value, x.TotalPages.Value));
    }
}