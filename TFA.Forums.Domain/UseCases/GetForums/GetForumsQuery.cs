using MediatR;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.UseCases.GetForums;

public record GetForumsQuery : IRequest<IEnumerable<Forum>>, IMonitoredRequest
{
    private const string CounterName = "forums.fetched";
    
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}