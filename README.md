# Turbohesap

Kurumsal ön muhasebe uygulaması için **.NET 10 + PostgreSQL + Blazor (Interactive Server)** tabanlı, token destekli tasarım sistemiyle gelen tam bir başlangıç şablonu.

> Arayüz dili **Türkçe**, kod (entity/alan/fonksiyon/tablo) dili **İngilizce**'dir.

## Çözüm yapısı

| Proje | Açıklama |
|-------|----------|
| `Turbohesap.Shared` | CQRS mesajları, DTO'lar, servis arayüzleri, FluentValidation kuralları, rol & sürüm sabitleri. EF/ASP.NET bağımlılığı yoktur. |
| `Turbohesap.Api` | Controller tabanlı API. EF Core (PostgreSQL, snake_case), Wolverine (CQRS + olay veriyolu), Audit.NET, Mapster, FluentValidation, JWT, OpenTelemetry, Serilog, Scalar + API sürümleme. |
| `Turbohesap.Web` | Blazor Interactive Server arayüzü. Token tabanlı tema sistemi (Vite + Tailwind v4 + TypeScript → `wwwroot/turbohesap.{css,js}`), Font Awesome. |

Ayrıntılı mimari için **[AGENTS.md](AGENTS.md)**, tasarım sistemi için **[DESIGN.md](DESIGN.md)**.

## Gereksinimler

- .NET SDK 10
- Node.js 20+ (frontend derlemesi)
- PostgreSQL 14+ (`turbohesap` veritabanı; bağlantı `src/Turbohesap.Api/appsettings.json`)

## Hızlı başlangıç

```bash
make install     # dotnet restore + npm install
make migrate     # veritabanı şemasını oluştur (ilk admin kullanıcısı tohumlanır)
make run         # frontend derlenir, API + Web başlar, URL'ler yazılır
```

| Adres | Açıklama |
|-------|----------|
| http://localhost:5180 | Web arayüzü |
| http://localhost:5020/scalar/v1 | API dokümantasyonu (Scalar) |
| http://localhost:5020/openapi/v1.json | OpenAPI belgesi |

Demo giriş: **admin@turbohesap.local / Admin123!**

Durdurmak için `make stop`. Tüm komutlar için `make help`.

## Öne çıkanlar

- **CQRS + olay veriyolu**: Controller → `IMessageBus` → handler → servis (transactional).
- **Küresel hata yakalama**: 500'ler `error_logs`'a (hash + tekrar sayısı); yanıt daima `{"detail":"…"}`.
- **EF seviyesinde denetim**: Audit.NET, değişiklikleri `audit_logs` tablosuna (jsonb) aynı transaction içinde yazar.
- **Tek kaynak doğrulama**: FluentValidation kuralları Shared'de; hem API hem Blazor kullanır.
- **Token tabanlı tema**: Açık/koyu mod + çalışma zamanı özelleştirme; tüm bileşenler `--th-*` token'larına bağlıdır.
- **Sekme tabanlı kabuk**: Sidebar (aranabilir/ağaç), AppBar, CommandLauncher, AppLauncher, AI paneli. Mobile-first.
