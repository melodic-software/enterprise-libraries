using Microsoft.AspNetCore.Routing;

namespace Enterprise.Api.Minimal.Mapping;

public interface IMapEndpoints
{
    public static abstract void MapEndpoints(IEndpointRouteBuilder builder);
}
