using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Options;

namespace Enterprise.Api.Startup.Delegates;

public delegate void ConfigureApi(WebApiOptions options, WebApiConfigEvents events);
