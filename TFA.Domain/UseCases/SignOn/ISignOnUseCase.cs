using TFA.Domain.Authentication;

namespace TFA.Domain.UseCases.SignOn;

public interface ISignOnUseCase
{
    Task<IIdentity> Execute(SignOnCommand command, CancellationToken cancellationToken);
}