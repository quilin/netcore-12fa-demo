using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;
using TFA.Search.Domain.UseCases.Index;
using TFA.Search.Domain.UseCases.Search;
using TFA.Search.Storage.Entities;
using TFA.Search.Storage.Storages;

namespace TFA.Search.Storage.DependencyInjection;

public static class SearchCollectionExtensions
{
    public static IServiceCollection AddSearchStorage(this IServiceCollection services,
        string connectionString)
    {
        services
            .AddScoped<IIndexStorage, IndexStorage>()
            .AddScoped<ISearchStorage, SearchStorage>();

        services.AddSingleton<IOpenSearchClient>(new OpenSearchClient(new Uri(connectionString))
        {
            ConnectionSettings =
            {
                DefaultIndices = { [typeof(SearchEntity)] = "tfa-search-v1" }
            }
        });

        return services;
    }
}