using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

namespace Enterprise.Api.SignalR.Options;

public class SignalROptions
{
    public const string ConfigSectionKey = "Custom:SignalR";

    public bool SignalREnabled { get; set; } = true;
    public Action<HubOptions>? ConfigureHubOptions { get; set; } = _ => { };
    public Action<IEndpointRouteBuilder>? MapHubs { get; set; } = _ => { };
}
