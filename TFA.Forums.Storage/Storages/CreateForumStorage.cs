using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.CreateForum;

namespace TFA.Forums.Storage.Storages;

internal class CreateForumStorage(
    IMemoryCache memoryCache,
    IGuidFactory guidFactory,
    ForumDbContext dbContext,
    IMapper mapper)
    : ICreateForumStorage
{
    public async Task<Forum> Create(string title, CancellationToken cancellationToken)
    {
        var forumId = guidFactory.Create();
        var forum = new Entities.Forum
        {
            ForumId = forumId,
            Title = title,
        };
        await dbContext.Forums.AddAsync(forum, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(nameof(GetForumsStorage.GetForums));

        return await dbContext.Forums
            .Where(f => f.ForumId == forumId)
            .ProjectTo<Forum>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}