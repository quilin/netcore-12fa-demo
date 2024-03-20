using MediatR;
using TFA.Search.Domain.Models;

namespace TFA.Search.Domain.UseCases.Search;

public record SearchQuery(string Query) : IRequest<(IEnumerable<SearchResult> resources, int totalCount)>;