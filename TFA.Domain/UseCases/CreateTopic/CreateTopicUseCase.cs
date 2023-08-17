using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICreateTopicStorage storage;

    public CreateTopicUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
    }

    public async Task<Topic> Execute(Guid forumId, string title, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        var forumExists = await storage.ForumExists(forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}