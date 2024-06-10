namespace Enterprise.Http.Redirection.Delegates;

public delegate void ForwardHeaders(HttpRequestMessage originalRequest, HttpRequestMessage redirectRequest);
