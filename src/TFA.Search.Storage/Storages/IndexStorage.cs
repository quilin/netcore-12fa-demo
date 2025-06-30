using OpenSearch.Client;
using TFA.Search.Domain.Models;
using TFA.Search.Domain.UseCases.Index;
using SearchEntity = TFA.Search.Storage.Entities.SearchEntity;

namespace TFA.Search.Storage.Storages;

internal class IndexStorage(IOpenSearchClient client) : IIndexStorage
{
    public async Task Index(Guid entityId, SearchEntityType entityType, string? title, string? text,
        CancellationToken cancellationToken)
    {
        await client.IndexAsync(new SearchEntity
        {
            EntityId = entityId,
            EntityType = (int)entityType,
            Title = title,
            Text = text,
        }, descriptor => descriptor, cancellationToken);
    }
}