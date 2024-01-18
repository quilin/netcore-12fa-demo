using TFA.API.Authentication;
using TFA.Domain.Authentication;

namespace TFA.API.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IAuthTokenStorage tokenStorage,
        IAuthenticationService authenticationService,
        IIdentityProvider identityProvider)
    {
        var identity = tokenStorage.TryExtract(httpContext, out var authToken)
            ? await authenticationService.Authenticate(authToken, httpContext.RequestAborted)
            : User.Guest;
        identityProvider.Current = identity;

        await next(httpContext);
    }
}