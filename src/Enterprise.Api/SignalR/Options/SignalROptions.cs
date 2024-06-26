using Microsoft.AspNetCore.Routing;

namespace Enterprise.Api.SignalR.Options;

public class SignalROptions
{
    public const string ConfigSectionKey = "Custom:SignalR";

    public bool SignalREnabled { get; set; } = true;
    public Action<IEndpointRouteBuilder>? MapHubs { get; set; } = _ => { };
}
