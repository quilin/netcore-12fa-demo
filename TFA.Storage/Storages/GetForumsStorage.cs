using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages;

internal class GetForumsStorage : IGetForumsStorage
{
    private readonly IMemoryCache memoryCache;
    private readonly ForumDbContext dbContext;

    public GetForumsStorage(
        IMemoryCache memoryCache,
        ForumDbContext dbContext)
    {
        this.memoryCache = memoryCache;
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) =>
        await memoryCache.GetOrCreateAsync<Domain.Models.Forum[]>(
            nameof(GetForums),
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return dbContext.Forums
                    .Select(f => new Domain.Models.Forum
                    {
                        Id = f.ForumId,
                        Title = f.Title
                    })
                    .ToArrayAsync(cancellationToken);
            });
}