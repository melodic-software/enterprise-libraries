using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Enterprise.Api.Startup.Options.Delegates;

public delegate void DeferredConfiguration(IConfiguration configuration, IWebHostEnvironment webHostEnvironment);
