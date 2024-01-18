using MediatR;
using TFA.Domain.Authentication;
using TFA.Domain.Monitoring;

namespace TFA.Domain.UseCases.SignIn;

public record SignInCommand(string Login, string Password) : IRequest<(IIdentity identity, string token)>, IMonitoredRequest
{
    private const string CounterName = "account.signedin";
    
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}