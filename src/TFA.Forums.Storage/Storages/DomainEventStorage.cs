using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using TFA.Forums.Domain.UseCases;
using TFA.Forums.Storage.Entities;
using ForumDomainEvent = TFA.Forums.Domain.DomainEvents.ForumDomainEvent;

namespace TFA.Forums.Storage.Storages;

internal class DomainEventStorage(
    ForumDbContext dbContext,
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    IMapper mapper) : IDomainEventStorage
{
    public async Task AddEvent(ForumDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var storageDomainEvent = mapper.Map<Models.ForumDomainEvent>(domainEvent);

        await dbContext.DomainEvents.AddAsync(new DomainEvent
        {
            DomainEventId = guidFactory.Create(),
            EmittedAt = momentProvider.Now,
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(storageDomainEvent),
            ActivityId = Activity.Current?.Id
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}