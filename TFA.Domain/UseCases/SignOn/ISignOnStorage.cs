namespace TFA.Domain.UseCases.SignOn;

public interface ISignOnStorage
{
    Task<Guid> CreateUser(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken);
}