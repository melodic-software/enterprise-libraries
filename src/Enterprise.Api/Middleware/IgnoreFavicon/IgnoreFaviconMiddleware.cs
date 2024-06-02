using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Middleware.IgnoreFavicon;
// https://stackoverflow.com/questions/67090158/how-to-ignore-favicon-call-in-asp-net-5-web-api

/// <summary>
/// Most browsers default to requesting a favicon whenever the user visits a new website.
/// This can trigger custom middleware an additional time, causing confusion.
/// 
/// </summary>
public class IgnoreFaviconMiddleware : IMiddleware
{
    private readonly ILogger<IgnoreFaviconMiddleware> _logger;

    /// <summary>
    /// Most browsers default to requesting a favicon whenever the user visits a new website.
    /// This can trigger custom middleware an additional time, causing confusion.
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public IgnoreFaviconMiddleware(ILogger<IgnoreFaviconMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.Value == "/favicon.ico")
        {
            // Here we're inspecting the path, and returning a 404.
            // This ensures subsequent middleware will not process the request.
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await next.Invoke(context);
    }
}
