using TFA.Forums.Domain.DomainEvents;

namespace TFA.Forums.Domain.UseCases;

public interface IDomainEventStorage : IStorage
{
    Task AddEvent(ForumDomainEvent domainEvent, CancellationToken cancellationToken);
}