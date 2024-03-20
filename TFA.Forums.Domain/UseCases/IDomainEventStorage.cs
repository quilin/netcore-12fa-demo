namespace TFA.Forums.Domain.UseCases;

public interface IDomainEventStorage : IStorage
{
    Task AddEvent<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken);
}