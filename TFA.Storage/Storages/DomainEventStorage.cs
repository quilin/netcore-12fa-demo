using System.Text.Json;
using TFA.Domain.UseCases;
using TFA.Storage.Entities;

namespace TFA.Storage.Storages;

internal class DomainEventStorage(
    ForumDbContext dbContext,
    IGuidFactory guidFactory,
    IMomentProvider momentProvider) : IDomainEventStorage
{
    public async Task AddEvent<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.DomainEvents.AddAsync(new DomainEvent
        {
            DomainEventId = guidFactory.Create(),
            EmittedAt = momentProvider.Now,
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(entity)
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}