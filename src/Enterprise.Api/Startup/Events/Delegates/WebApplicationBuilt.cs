using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Startup.Events.Delegates;

public delegate Task WebApplicationBuilt(WebApplication app);
