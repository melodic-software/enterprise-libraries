namespace Enterprise.Constants;

public static class RequestHeaderConstants
{
    /// <summary>
    /// The Sec-CH-UA header, short for "Client Hints User-Agent", is part of the User-Agent Client Hints (UA-CH) feature.
    /// It provides a more privacy-conscious and structured way for browsers to communicate information
    /// about the user's device and browser to web servers.
    /// </summary>
    public const string SecChUa = "Sec-CH-UA";

    /// <summary>
    /// The X-Forwarded-For header is used to identify the originating IP address of a client connecting to
    /// a web server through an HTTP proxy or load balancer.
    /// </summary>
    public const string XForwardedFor = "X-Forwarded-For";

    /// <summary>
    /// The x-client-ssl-protocol header is often used to specify the SSL/TLS protocol version that a client used to establish a connection.
    /// This can be useful for logging, analytics, or enforcing security policies.
    /// </summary>
    public const string XClientSslProtocol = "x-client-ssl-protocol";
}