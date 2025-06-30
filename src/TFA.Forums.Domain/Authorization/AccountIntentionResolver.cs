using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization.AccessManagement;

namespace TFA.Forums.Domain.Authorization;

internal class AccountIntentionResolver : IIntentionResolver<AccountIntention>
{
    public bool IsAllowed(IIdentity subject, AccountIntention intention) => intention switch
    {
        AccountIntention.SignOut => subject.IsAuthenticated(),
        _ => false,
    };
}