using MediatR;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.GetForums;

namespace TFA.Forums.Domain.UseCases.GetTopics;

internal class GetTopicsUseCase(
    IGetForumsStorage getForumsStorage,
    IGetTopicsStorage storage)
    : IRequestHandler<GetTopicsQuery, (IEnumerable<Topic> resources, int totalCount)>
{
    public async Task<(IEnumerable<Topic> resources, int totalCount)> Handle(
        GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await getForumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);
        return await storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
    }
}