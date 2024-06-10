using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Enterprise.Api.Startup.Options.Delegates;

public delegate void Configure<in TOptions>(TOptions options, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    where TOptions : class, new();
