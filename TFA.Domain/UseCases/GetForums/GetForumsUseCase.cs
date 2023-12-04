using TFA.Domain.Monitoring;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IGetForumsStorage storage;
    private readonly DomainMetrics metrics;

    public GetForumsUseCase(
        IGetForumsStorage storage,
        DomainMetrics metrics)
    {
        this.storage = storage;
        this.metrics = metrics;
    }

    public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
    {
        try
        {
            var result = await storage.GetForums(cancellationToken);
            metrics.ForumsFetched(true);
            return result;
        }
        catch
        {
            metrics.ForumsFetched(false);
            throw;
        }
    }
}