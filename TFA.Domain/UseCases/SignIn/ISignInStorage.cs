namespace TFA.Domain.UseCases.SignIn;

public interface ISignInStorage
{
    Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken);

    Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken);
}