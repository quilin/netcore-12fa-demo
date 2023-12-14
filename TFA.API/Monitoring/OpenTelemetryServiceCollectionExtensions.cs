using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TFA.API.Monitoring;

internal static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddMeter("TFA.Domain")
                .AddPrometheusExporter())
            .WithTracing(builder => builder
                .ConfigureResource(r => r.AddService("TFA"))
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(cfg => cfg.SetDbStatementForText = true)
                .AddSource("TFA.Domain")
                .AddConsoleExporter()
                .AddJaegerExporter(options =>
                    options.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));

        return services;
    }
}