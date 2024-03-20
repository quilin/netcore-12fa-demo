using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.GetForums;

namespace TFA.Forums.Storage.Storages;

internal class GetForumsStorage(
    IMemoryCache memoryCache,
    ForumDbContext dbContext,
    IMapper mapper)
    : IGetForumsStorage
{
    public async Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken) =>
        (await memoryCache.GetOrCreateAsync<Forum[]>(
            nameof(GetForums),
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return dbContext.Forums
                    .ProjectTo<Forum>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
            }))!;
}