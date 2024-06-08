using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Startup.Events.Delegates;

public delegate Task RequestPipelineConfigured(WebApplication app);
