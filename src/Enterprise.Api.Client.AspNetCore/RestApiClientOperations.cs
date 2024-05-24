using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Client.AspNetCore;

public static class RestApiClientOperations
{
    public static async Task HandleInvalidResponse(HttpResponseMessage response, HttpClient httpClient, Uri resourceUri, ILogger logger, HttpContext? httpContext = null, ClaimsPrincipal? claimsPrincipal = null)
    {
        try
        {
            // Set the access token for the HttpClient if httpContext is provided.
            await SetAccessToken(httpClient, httpContext);

            // Check if the response indicates a failure
            await HandleResponse(response, httpClient, resourceUri, logger, claimsPrincipal);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling the invalid response.");
        }
    }

    private static async Task SetAccessToken(HttpClient httpClient, HttpContext? httpContext)
    {
        if (httpContext == null)
        {
            return;
        }

        string? accessToken = await httpContext.GetTokenAsync("access_token");

        if (!string.IsNullOrEmpty(accessToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        // For a better way to include and manage access tokens for API calls:
        // https://identitymodel.readthedocs.io/en/latest/aspnetcore/web.html
    }

    private static async Task HandleResponse(HttpResponseMessage response, HttpClient httpClient, Uri resourceUri,
        ILogger logger, ClaimsPrincipal? claimsPrincipal)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        int statusCode = (int)response.StatusCode;

        string fullPath = httpClient.BaseAddress != null
            ? new Uri(httpClient.BaseAddress, resourceUri).ToString()
            : resourceUri.ToString();

        // Attempt to read ProblemDetails from the response content
        var details = new ProblemDetails();

        try
        {
            details = await response.Content.ReadFromJsonAsync<ProblemDetails>() ?? new ProblemDetails();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while reading ProblemDetails from the response content.");
        }

        string traceId = details.Extensions.TryGetValue("traceId", out object? traceIdObj)
            ? traceIdObj?.ToString() ?? string.Empty
            : string.Empty;

        string? username = claimsPrincipal?.Identity?.IsAuthenticated ?? false
            ? claimsPrincipal.Identity.Name
            : string.Empty;

        // Log the warning with details of the failure
        logger.LogWarning(
            "API Failure: {FullPath}, " +
            "API Response: {ApiResponse}, " +
            "Trace: {Trace}, " +
            "Username: {Username}",
            fullPath, statusCode, traceId, username
        );
    }
}
