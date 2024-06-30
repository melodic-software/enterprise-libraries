using Microsoft.Extensions.Configuration;

namespace Enterprise.Api.Startup.Options.Abstract;

public interface IConfigureWebApiOptions
{
    public static abstract void Configure(WebApiOptions options, IConfiguration configuration);
}
