using FluentValidation;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICreateTopicStorage storage;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
    }

    public async Task<Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (forumId, title) = command;
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        var forumExists = await storage.ForumExists(forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}