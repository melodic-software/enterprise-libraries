using Microsoft.AspNetCore.Routing;

namespace Enterprise.Api.Minimal.Mapping;

public interface IMapEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder);
}
