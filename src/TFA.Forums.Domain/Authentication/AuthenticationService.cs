using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TFA.Forums.Domain.Authentication;

internal class AuthenticationService(
    ISymmetricDecryptor decryptor,
    IAuthenticationStorage storage,
    ILogger<AuthenticationService> logger,
    IOptions<AuthenticationConfiguration> options)
    : IAuthenticationService
{
    private readonly AuthenticationConfiguration configuration = options.Value;

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        string sessionIdString;
        try
        {
            sessionIdString = await decryptor.Decrypt(authToken, configuration.Key, cancellationToken);
        }
        catch (CryptographicException cryptographicException)
        {
            logger.LogWarning(
                cryptographicException,
                "Cannot decrypt auth token, maybe someone is trying to forge it");
            return User.Guest;
        }

        if (!Guid.TryParse(sessionIdString, out var sessionId))
        {
            return User.Guest;
        }

        var session = await storage.FindSession(sessionId, cancellationToken);
        if (session is null)
        {
            return User.Guest;
        }

        if (session.ExpiresAt < DateTimeOffset.UtcNow)
        {
            return User.Guest;
        }

        return new User(session.UserId, sessionId);
    }
}