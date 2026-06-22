using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// OpenTelemetry izleme ve metrik yapılandırması (req 9). ASP.NET Core, HttpClient,
/// PostgreSQL (Npgsql ActivitySource) ve çalışma zamanı enstrümantasyonu eklenir.
/// OTLP dışa aktarımı yalnızca "Telemetry:OtlpEndpoint" tanımlıysa etkinleşir.
/// </summary>
public static class TelemetryConfiguration
{
    /// <summary>Uygulamanın kendi izleri için ActivitySource (servislerde kullanılabilir).</summary>
    public const string ServiceName = "Turbohesap.Api";

    public static readonly ActivitySource ActivitySource = new(ServiceName);

    public static WebApplicationBuilder AddTelemetry(this WebApplicationBuilder builder)
    {
        var otlpEndpoint = builder.Configuration["Telemetry:OtlpEndpoint"];

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ServiceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(ServiceName)
                    .AddSource("Npgsql")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    tracing.AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint));
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    metrics.AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint));
                }
            });

        return builder;
    }
}
