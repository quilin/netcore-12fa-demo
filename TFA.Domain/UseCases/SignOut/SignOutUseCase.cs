using MediatR;
using TFA.Domain.Authentication;

namespace TFA.Domain.UseCases.SignOut;

internal class SignOutUseCase(
    IIdentityProvider identityProvider,
    ISignOutStorage storage)
    : IRequestHandler<SignOutCommand>
{
    public Task Handle(SignOutCommand command, CancellationToken cancellationToken) => 
        storage.RemoveSession(identityProvider.Current.SessionId, cancellationToken);
}