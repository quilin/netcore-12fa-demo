namespace TFA.Forums.API.Authentication;

internal class AuthTokenStorage : IAuthTokenStorage
{
    private const string HeaderKey = "TFA-Auth-Token";

    public bool TryExtract(HttpContext httpContext, out string token)
    {
        if (httpContext.Request.Cookies.TryGetValue(HeaderKey, out var value) &&
            !string.IsNullOrWhiteSpace(value))
        {
            token = value;
            return true;
        }

        token = string.Empty;
        return false;
    }

    public void Store(HttpContext httpContext, string token) =>
        httpContext.Response.Cookies.Append(HeaderKey, token);
}