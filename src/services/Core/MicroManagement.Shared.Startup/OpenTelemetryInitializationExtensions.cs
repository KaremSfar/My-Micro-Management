using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroManagement.Shared;

public static class OpenTelemetryInitializationExtensions
{
    public static void AddOpenTelemetry(this IServiceCollection services, string serviceName, string otelEndpoint)
    {
        if (string.IsNullOrWhiteSpace(otelEndpoint))
            return;

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName);

        var ConfigureOtlpExporter = (OtlpExporterOptions options) => { options.Endpoint = new Uri(otelEndpoint); };

        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddOtlpExporter(ConfigureOtlpExporter);
            }).WithLogging(loggerOptions =>
            {
                loggerOptions
                    .SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(ConfigureOtlpExporter);
            }).WithMetrics(metricProviderBuilder =>
            {
                metricProviderBuilder.SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(ConfigureOtlpExporter);
            });
    }
}