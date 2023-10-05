using Microsoft.Extensions.Options;

namespace TFA.Domain.Authentication;

internal class AuthenticationService : IAuthenticationService
{
    private readonly ISymmetricDecryptor decryptor;
    private readonly AuthenticationConfiguration configuration;

    public AuthenticationService(
        ISymmetricDecryptor decryptor,
        IOptions<AuthenticationConfiguration> options)
    {
        this.decryptor = decryptor;
        configuration = options.Value;
    }

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        var userIdString = await decryptor.Decrypt(authToken, configuration.Key, cancellationToken);
        // TODO: verify user identifier
        return new User(Guid.Parse(userIdString));
    }
}