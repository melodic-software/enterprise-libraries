using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Startup.Events.Delegates;

public delegate Task BuilderCreated(WebApplicationBuilder builder);
