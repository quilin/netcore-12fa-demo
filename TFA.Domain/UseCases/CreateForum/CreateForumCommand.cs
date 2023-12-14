using MediatR;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;

namespace TFA.Domain.UseCases.CreateForum;

public record CreateForumCommand(string Title) : IRequest<Forum>, IMonitoredRequest
{
    private const string CounterName = "forums.created";

    public void MonitorSuccess(DomainMetrics metrics) =>
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}