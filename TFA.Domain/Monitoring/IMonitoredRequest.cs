namespace TFA.Domain.Monitoring;

internal interface IMonitoredRequest
{
    void MonitorSuccess(DomainMetrics metrics);
    void MonitorFailure(DomainMetrics metrics);
}