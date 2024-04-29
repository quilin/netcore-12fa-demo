using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.CreateTopic;

namespace TFA.Forums.Storage.Storages;

internal class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMapper mapper,
    IMomentProvider momentProvider,
    ForumDbContext dbContext)
    : ICreateTopicStorage
{
    public async Task<Topic> CreateTopic(Guid forumId, Guid userId, string title,
        CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var topic = new Entities.Topic
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
            .ProjectTo<Topic>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}