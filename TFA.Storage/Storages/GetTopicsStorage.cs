using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Storage.Storages;

internal class GetTopicsStorage(ForumDbContext dbContext) : IGetTopicsStorage
{
    public async Task<(IEnumerable<Domain.Models.Topic> resources, int totalCount)> GetTopics(
        Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);
        var resources = await query
            .Select(t => new Domain.Models.Topic
            {
                Id = t.TopicId,
                ForumId = t.ForumId,
                UserId = t.UserId,
                Title = t.Title,
                CreatedAt = t.CreatedAt
            })
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}