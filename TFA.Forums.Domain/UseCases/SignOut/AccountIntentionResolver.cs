using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization;

namespace TFA.Forums.Domain.UseCases.SignOut;

internal class AccountIntentionResolver : IIntentionResolver<AccountIntention>
{
    public bool IsAllowed(IIdentity subject, AccountIntention intention) => intention switch
    {
        AccountIntention.SignOut => subject.IsAuthenticated(),
        _ => false,
    };
}