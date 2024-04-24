using System.Net;
using Enterprise.Http.Extensions;

namespace Enterprise.Http.Redirection;

public static class HttpClientRedirectionService
{
    public static async Task<HttpResponseMessage> SendRequestWithRedirects(
        HttpClient httpClient,
        HttpRequestMessage request,
        Action<HttpRequestMessage, HttpRequestMessage>? forwardHeaders = null,
        int maxRedirects = 10)
    {
        forwardHeaders ??= DefaultHeaderForwarder.ForwardHeaders;

        HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        int redirectCount = 0;

        try
        {
            while (response.IsRedirect() && redirectCount < maxRedirects)
            {
                Uri? redirectUri = response.Headers.Location;

                if (redirectUri == null)
                    throw new InvalidOperationException("Redirect location is null.");

                if (!redirectUri.IsAbsoluteUri)
                    redirectUri = new Uri(request.RequestUri!, redirectUri);

                HttpRequestMessage originalRequest = request;

                request = new HttpRequestMessage(originalRequest.Method, redirectUri);

                forwardHeaders(originalRequest, request);

                if (response.StatusCode == HttpStatusCode.SeeOther)
                {
                    request.Method = HttpMethod.Get;
                    request.Content = null;
                }

                response.Dispose();
                response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                redirectCount++;
            }

            if (redirectCount >= maxRedirects)
                throw new InvalidOperationException("Maximum redirect limit reached.");
        }
        catch
        {
            response.Dispose();
            throw;
        }

        return response;
    }
}