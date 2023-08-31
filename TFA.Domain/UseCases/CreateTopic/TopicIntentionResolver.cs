using TFA.Domain.Authentication;
using TFA.Domain.Authorization;

namespace TFA.Domain.UseCases.CreateTopic;

internal class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
    {
        TopicIntention.Create => subject.IsAuthenticated(),
        _ => false
    };
}