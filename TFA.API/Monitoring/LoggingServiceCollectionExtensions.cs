using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace TFA.API.Monitoring;

internal static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddApiLogging(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment) =>
        services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithProperty("Application", "TFA.API")
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Enrich.With<TraceEnricher>()
                .WriteTo.GrafanaLoki(
                    configuration.GetConnectionString("Logs")!,
                    propertiesAsLabels: ["Application", "Environment"]))
            .CreateLogger()));
}

internal class TraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current ?? default;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity?.TraceId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity?.SpanId));
    }
}