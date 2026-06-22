using Serilog;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// Serilog yapılandırması (req 7). Sink ve seviye ayarları appsettings.json'dan okunur;
/// böylece kod değişmeden konsol/dosya hedefleri ayarlanabilir.
/// </summary>
public static class SerilogConfiguration
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

        return builder;
    }
}
