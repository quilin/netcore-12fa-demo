using Microsoft.EntityFrameworkCore;
using TFA.Domain;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Storage.Storages;

public class CreateTopicStorage : ICreateTopicStorage
{
    private readonly IGuidFactory guidFactory;
    private readonly IMomentProvider momentProvider;
    private readonly ForumDbContext dbContext;

    public CreateTopicStorage(
        IGuidFactory guidFactory,
        IMomentProvider momentProvider,
        ForumDbContext dbContext)
    {
        this.guidFactory = guidFactory;
        this.momentProvider = momentProvider;
        this.dbContext = dbContext;
    }

    public Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken) =>
        dbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);

    public async Task<Domain.Models.Topic> CreateTopic(Guid forumId, Guid userId, string title,
        CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var topic = new Topic
        {
            TopicId = topicId,
            ForumId = forumId,
            UserId = userId,
            Title = title,
            CreatedAt = momentProvider.Now,
        };

        await dbContext.Topics.AddAsync(topic, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Topics
            .Where(t => t.TopicId == topicId)
            .Select(t => new Domain.Models.Topic
            {
                Id = t.TopicId,
                ForumId = t.ForumId,
                UserId = t.UserId,
                Title = t.Title,
                CreatedAt = t.CreatedAt
            })
            .FirstAsync(cancellationToken);
    }
}