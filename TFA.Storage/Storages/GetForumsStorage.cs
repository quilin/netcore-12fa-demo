using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages;

public class GetForumsStorage : IGetForumsStorage
{
    private readonly ForumDbContext dbContext;

    public GetForumsStorage(
        ForumDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) =>
        await dbContext.Forums
            .Select(f => new Domain.Models.Forum
            {
                Id = f.ForumId,
                Title = f.Title
            })
            .ToArrayAsync(cancellationToken);
}