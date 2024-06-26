using Microsoft.AspNetCore.Routing;

namespace Enterprise.Api.SignalR.Options;

internal class SignalROptions
{
    public const string ConfigSectionKey = "Custom:ModularMonolith";

    public bool SignalREnabled { get; set; } = true;
    public Action<IEndpointRouteBuilder>? MapHubs { get; set; } = _ => { };
}
