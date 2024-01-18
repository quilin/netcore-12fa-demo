using MediatR;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;

namespace TFA.Domain.UseCases.SignOut;

internal class SignOutUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ISignOutStorage storage)
    : IRequestHandler<SignOutCommand>
{
    public Task Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntention.SignOut);

        var sessionId = identityProvider.Current.SessionId;
        return storage.RemoveSession(sessionId, cancellationToken);
    }
}