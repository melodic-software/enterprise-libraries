namespace Enterprise.Http.Redirection;

public static class DefaultHeaderForwarder
{
    public static void ForwardHeaders(HttpRequestMessage originalRequest, HttpRequestMessage redirectRequest)
    {
        foreach (KeyValuePair<string, IEnumerable<string>> header in originalRequest.Headers)
        {
            if (!ShouldHeaderBeForwarded(header.Key))
            {
                continue;
            }

            redirectRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (originalRequest.Content == null)
        {
            return;
        }

        redirectRequest.Content ??= new ByteArrayContent(Array.Empty<byte>());

        foreach (KeyValuePair<string, IEnumerable<string>> header in originalRequest.Content.Headers)
        {
            redirectRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    private static bool ShouldHeaderBeForwarded(string headerKey)
    {
        // Common headers that shouldn't be forwarded on redirect
        HashSet<string> excludedHeaders = ["Host", "Content-Length", "Content-Type"];

        return !excludedHeaders.Contains(headerKey);
    }
}
