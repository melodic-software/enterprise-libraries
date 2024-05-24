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

        while (response.IsRedirect() && redirectCount < maxRedirects)
        {
            Uri? redirectUri = response.Headers.Location;

            if (redirectUri == null)
            {
                throw new InvalidOperationException("Redirect location is null.");
            }

            if (!redirectUri.IsAbsoluteUri)
            {
                redirectUri = new Uri(request.RequestUri!, redirectUri);
            }

            using HttpRequestMessage originalRequest = request;
            using var newRequest = new HttpRequestMessage(originalRequest.Method, redirectUri);

            forwardHeaders(originalRequest, newRequest);

            if (response.StatusCode == HttpStatusCode.SeeOther)
            {
                newRequest.Method = HttpMethod.Get;
                newRequest.Content = null;
            }

            response.Dispose();
            response = await httpClient.SendAsync(newRequest, HttpCompletionOption.ResponseHeadersRead);

            request = newRequest;
            redirectCount++;
        }

        if (redirectCount >= maxRedirects)
        {
            throw new InvalidOperationException("Maximum redirect limit reached.");
        }

        return response;
    }
}
