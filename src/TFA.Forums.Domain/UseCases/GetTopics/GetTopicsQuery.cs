using MediatR;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.UseCases.GetTopics;

public record GetTopicsQuery(Guid ForumId, int Skip, int Take) : IRequest<(IEnumerable<Topic> resources, int totalCount)>, IMonitoredRequest
{
    private const string CounterName = "topics.fetched";
    
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}