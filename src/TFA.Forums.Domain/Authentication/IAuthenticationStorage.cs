namespace TFA.Forums.Domain.Authentication;

public interface IAuthenticationStorage
{
    Task<Session?> FindSession(Guid sessionId, CancellationToken cancellationToken);
}