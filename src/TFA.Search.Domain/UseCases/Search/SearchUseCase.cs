using MediatR;
using TFA.Search.Domain.Models;

namespace TFA.Search.Domain.UseCases.Search;

internal class SearchUseCase(ISearchStorage storage)
    : IRequestHandler<SearchQuery, (IEnumerable<SearchResult> resources, int totalCount)>
{
    public Task<(IEnumerable<SearchResult> resources, int totalCount)> Handle(
        SearchQuery request, CancellationToken cancellationToken) =>
        storage.Search(request.Query, cancellationToken);
}