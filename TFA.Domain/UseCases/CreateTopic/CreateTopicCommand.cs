using MediatR;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;

namespace TFA.Domain.UseCases.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title) : IRequest<Topic>, IMonitoredRequest
{
    private const string CounterName = "topics.created";

    public void MonitorSuccess(DomainMetrics metrics) =>
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}