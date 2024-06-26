using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Enterprise.Api.SignalR.Hubs;

[Authorize]
public class HubBase<T> : Hub<T> where T : class
{
    
}
