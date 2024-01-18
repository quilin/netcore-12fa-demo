using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage.Entities;

namespace TFA.Storage.Storages;

internal class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    ForumDbContext dbContext)
    : ICreateTopicStorage
{
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