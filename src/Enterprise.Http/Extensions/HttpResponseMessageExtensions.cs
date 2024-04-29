using System.Net;

namespace Enterprise.Http.Extensions;

public static class HttpResponseMessageExtensions
{
    public static bool IsRedirect(this HttpResponseMessage? response)
    {
        if (response == null)
            return false;

        switch (response.StatusCode)
        {
            case HttpStatusCode.MovedPermanently: // 301
            case HttpStatusCode.Redirect: // 302
            case HttpStatusCode.RedirectMethod: // 303
            case HttpStatusCode.TemporaryRedirect: // 307
            //case HttpStatusCode.RedirectKeepVerb: // 307
            case HttpStatusCode.PermanentRedirect: // 308
                return response.Headers.Location != null;
            default:
                return false;
        }
    }
}