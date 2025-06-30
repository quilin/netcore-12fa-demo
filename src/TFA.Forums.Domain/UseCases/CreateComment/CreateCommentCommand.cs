using MediatR;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.UseCases.CreateComment;

public record CreateCommentCommand(Guid TopicId, string Text) : IRequest<Comment>, IMonitoredRequest
{
    private const string CounterName = "comments.created";
    
    public void MonitorSuccess(DomainMetrics metrics) =>
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) =>
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}