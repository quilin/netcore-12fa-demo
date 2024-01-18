using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Context.Propagation;

namespace TFA.Domain.Monitoring;

internal abstract class MonitoringPipelineBehavior
{
    protected static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
}

internal class MonitoringPipelineBehavior<TRequest, TResponse>(
    ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger,
    DomainMetrics metrics)
    : MonitoringPipelineBehavior, IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IMonitoredRequest monitoredRequest) return await next.Invoke();

        using var activity = DomainMetrics.ActivitySource.StartActivity(
            "usecase", ActivityKind.Internal, default(ActivityContext));

        activity?.AddTag("tfa.command", request.GetType().Name);

        try
        {
            var result = await next.Invoke();
            monitoredRequest.MonitorSuccess(metrics);
            activity?.AddTag("error", false);
            return result;
        }
        catch (Exception e)
        {
            monitoredRequest.MonitorFailure(metrics);
            activity?.AddTag("error", true);
            logger.LogError(e, "Unhandled error caught while handling command {Command}", request);
            throw;
        }
    }
}