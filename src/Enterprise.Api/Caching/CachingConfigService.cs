using Enterprise.Api.Caching.Services;
using Enterprise.Redis.Config;
using Marvin.Cache.Headers;
using Marvin.Cache.Headers.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Caching;

public static class CachingConfigService
{
    public static void ConfigureCaching(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        // Uncomment for in memory (local/non-distributed) caching.
        //services.AddMemoryCache();

        // Configuration for "IDistributedCache" references.

        services.ConfigureRedis(environment, configuration);

        // This is an alternate configuration for "IDistributedCache" (not likely going to be used).
        // SQL traffic is not reduced - REDIS is better served for reducing SQL overhead.
        // TODO: Make this configurable.
        //services.AddDistributedSqlServerCache(options =>
        //{
        //    string cacheConnectionString = string.Empty;
        //    string cacheSchema = "dbo";
        //    string cacheTableName = "Cache";

        //    options.ConnectionString = cacheConnectionString;
        //    options.SchemaName = cacheSchema;
        //    options.TableName = cacheTableName;
        //});
    }

    public static void ConfigureResponseCaching(this IServiceCollection services)
    {
        // TODO: Add options to make this configurable.

        // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response
        // NOTE: There are several requirements for response caching to work at an individual request level.
        // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/middleware
        //services.AddResponseCaching();

        // https://github.com/KevinDockx/HttpCacheHeaders
        // https://www.nuget.org/packages/Marvin.Cache.Headers
        // This is a custom package that adds support for ETags.
        // It allows for the addition of HttpCache headers to responses (Cache-Control, Expires, ETag, Last-Modified)
        // and implements cache expiration and validation models.

        // TODO: Configure a REDIS cache provider.
        // TODO: Does this take into account requests with different custom version header values?

        // Register services required by any custom provider delegates.
        // These should only apply to "AddHttpCacheHeaders".
        services.AddTransient<CustomStoreKeyGenerator>();
        services.AddTransient<CustomETagGenerator>();

        services.AddHttpCacheHeaders(expirationModelOptions =>
            {
                // This sets the "Cache-Control" response header (globally).
                // They can be set on the controller level by using the [HttpCacheExpiration] attribute.

                expirationModelOptions.MaxAge = 60;
                expirationModelOptions.CacheLocation = CacheLocation.Private;
            }, validationModelOptions =>
            {
                // These are global options.
                // They can be overridden at the controller/action level by using the [HttpCacheValidation] attribute.

                // Tells a cache that re-validation has to happen if a response becomes stale.
                // This forces re-validation by the cache even if the client has decided that stale responses are OK for a specified amount of time.
                // Clients specify this by setting the Max-Stale directive.

                validationModelOptions.MustRevalidate = true;
            },
            middlewareOptions => middlewareOptions.IgnoredStatusCodes = [..HttpStatusCodes.AllErrors], 
            storeKeyGeneratorFunc: provider => provider.GetRequiredService<CustomStoreKeyGenerator>(),
            eTagGeneratorFunc: provider => provider.GetRequiredService<CustomETagGenerator>());

        // NOTE: ETags can be used as tokens/validators for optimistic concurrency strategy.
        // Either the client (or preferably intermediary cache server or CDN) sends an "If-Match" header
        // When there is a mismatch, a 412 Precondition Failed will be returned.
    }

    public static void ConfigureOutputCaching(this IServiceCollection services)
    {
        // Uses e-tags and memory cache by default (non-distributed).
        // For distributed caching, a custom implementation of "IOutputCacheStore" must be provided.
        // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0
    }

    public static void UseCaching(this WebApplication app)
    {
        // TODO: Add options to make this configurable.

        //UseResponseCompression(app);
        //UseResponseCaching(app);
        UseHttpCacheHeaders(app);
    }

    private static void UseResponseCompression(WebApplication app)
    {
        // The compression modules in IIS are better than the .NET core implementations.
        // https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-8.0
        // Uncomment if hosting directly on an HTTP.sys or Kestrel server.
        //app.UseResponseCompression();
    }

    private static void UseResponseCaching(WebApplication app)
    {
        // Response caching needs to be placed before controllers are added (which adds endpoint mapping).
        // We want to ensure that the cache middleware can serve something up before the MVC logic is routed to / executed.
        //app.UseResponseCaching();
    }

    private static void UseHttpCacheHeaders(WebApplication app)
    {
        // Adds support for ETags, must be added after response caching.
        // https://github.com/KevinDockx/HttpCacheHeaders
        // https://www.nuget.org/packages/Marvin.Cache.Headers
        // TODO: does this take into account requests with different custom version header values?
        app.UseHttpCacheHeaders();
    }
}
