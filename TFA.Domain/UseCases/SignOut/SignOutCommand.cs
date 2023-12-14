using MediatR;
using TFA.Domain.Monitoring;

namespace TFA.Domain.UseCases.SignOut;

public record SignOutCommand : IRequest, IMonitoredRequest
{
    private const string CounterName = "user.sign-out";

    public void MonitorSuccess(DomainMetrics metrics) =>
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) =>
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}