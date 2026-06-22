using MapsterMapper;
using Turbohesap.Api.Persistence;
using Wolverine;
using Wolverine.FluentValidation;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// Wolverine'i CQRS aracısı (mediator) ve olay veriyolu olarak yapılandırır (req 6).
/// Controller'lar <see cref="IMessageBus"/> üzerinden komut/sorgu gönderir; handler'lar
/// servis katmanına delege eder. FluentValidation middleware'i handler'dan önce çalışır.
/// </summary>
public static class MessagingConfiguration
{
    public static WebApplicationBuilder AddMessaging(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(options =>
        {
            // Handler/middleware kodunu çalışma zamanında derler (Roslyn). Geliştirme için
            // en kolay yol; üretimde 'dotnet run -- codegen write' ile statik üretim tercih edilebilir.
            options.UseRuntimeCompilation();

            // Bazı altyapı tipleri "opak" fabrikalarla kaydedilir (EF Core DbContext bir scoped
            // fabrika, Mapster IMapper ise IServiceProvider sarmalar). Wolverine bunları üretilen
            // kod içinde inşa edemez; scoped IServiceProvider'dan çözülmelerini istiyoruz. Bu sayede
            // ince handler'lar, bu altyapıya bağımlı iş servislerini sorunsuz çağırır. Yeni bir opak
            // bağımlılık eklersen buraya bir satır eklemen yeterli.
            options.CodeGeneration.AlwaysUseServiceLocationFor<AppDbContext>();
            options.CodeGeneration.AlwaysUseServiceLocationFor<IMapper>();

            // Komut/sorgu mesajları handler çalışmadan önce FluentValidation ile doğrulanır.
            options.UseFluentValidation();
        });

        return builder;
    }
}
