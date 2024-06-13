using Enterprise.Api.Startup.Options;

namespace Enterprise.Api.Core.Startup.Options.Abstract;

public interface IConfigureWebApiOptions
{
    public static abstract void Configure(WebApiOptions options);
}
