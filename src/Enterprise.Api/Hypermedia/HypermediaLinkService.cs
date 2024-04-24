using Enterprise.Api.Client.Hypermedia;
using Enterprise.Api.Client.Hypermedia.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Api.Hypermedia;

public class HypermediaLinkService
{
    public static HypermediaLinkDto CreateSelfLink(string? href)
    {
        return new HypermediaLinkDto(href, StandardRelations.Self, HttpMethods.Get);
    }

    public static HypermediaLinkDto CreateSelfLink(ControllerBase controller, string routeName, object? values)
    {
        return CreateLink(controller, routeName, values, StandardRelations.Self, HttpMethods.Get);
    }

    public static HypermediaLinkDto CreateSelfLink(IUrlHelper urlHelper, string routeName, object? values)
    {
        return CreateLink(urlHelper, routeName, values, StandardRelations.Self, HttpMethods.Get);
    }

    public static HypermediaLinkDto CreateLink(ControllerBase controller, string routeName, object? values, string rel, string method)
    {
        return CreateLink(controller.Url, routeName, values, rel, method);
    }

    public static HypermediaLinkDto CreateLink(IUrlHelper urlHelper, string routeName, object? values, string rel, string method)
    {
        string? href = urlHelper.Link(routeName, values);
        HypermediaLinkDto linkModel = new HypermediaLinkDto(href, rel, method);
        return linkModel;
    }
}