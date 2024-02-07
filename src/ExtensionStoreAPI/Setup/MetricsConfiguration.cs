using OpenTelemetry.Metrics;

namespace ExtensionStoreAPI.Setup;

public static class MetricsConfiguration
{
    public static void AddMetricsConfiguration(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddPrometheusExporter();
            });
    }
}