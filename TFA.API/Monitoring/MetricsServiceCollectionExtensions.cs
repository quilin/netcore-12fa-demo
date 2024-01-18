using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TFA.API.Monitoring;

internal static class MetricsServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment builderEnvironment)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddMeter("TFA.Domain")
                .AddPrometheusExporter())
            .WithTracing(builder => builder
                .ConfigureResource(r => r.AddService("TFA"))
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter += context =>
                        !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) &&
                        !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
                    options.EnrichWithHttpResponse = (activity, response) =>
                        activity.AddTag("error", response.StatusCode >= 400);
                })
                .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                .AddJaegerExporter(cfg => cfg.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));

        return services;
    }
}