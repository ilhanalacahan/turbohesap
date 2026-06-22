# AGENTS.md — Turbohesap Mimari ve Geliştirme Rehberi

Bu belge, Turbohesap çözümünün mimarisini, kurallarını ve yeni özelliklerin nasıl ekleneceğini tarif eder. Hem insan geliştiriciler hem de yapay zeka ajanları bu belgeyi tek kaynak olarak kullanabilir. Tasarım sistemi için ayrıca **[DESIGN.md](DESIGN.md)**'e bakın.

---

## 1. Temel ilkeler

1. **Dil ayrımı**: Arayüze görünen her şey **Türkçe**; kodda her şey (entity, alan, fonksiyon, tablo, kolon) **İngilizce**.
2. **Mimari yok, dizin var**: API ve Web katmanlı mimari (Clean/Onion) kullanmaz; sorumluluklar **dizinlerle** ayrılır.
3. **Sözleşmeler Shared'de**: DTO, CQRS mesajı, servis arayüzü ve doğrulama kuralı yalnızca `Turbohesap.Shared`'dedir. Böylece API ve Web aynı tipleri ve aynı doğrulamayı paylaşır.
4. **İş mantığı servis katmanında ve transactional**: Tüm yazma işlemleri servis içinde açık transaction ile yapılır; okuma işlemleri `AsNoTracking` kullanır.
5. **Token tabanlı arayüz**: Hiçbir bileşen sabit renk/ölçü içermez; her şey `--th-*` token'larına bağlıdır (renk, gölge, **yazı tipi**, **yoğunluk** dahil — DESIGN.md). Tema/şablon tasarımcısı bir **drawer**'dır ve değişiklikleri gerçek zamanlı uygular.
6. **Mobile-first**: Arayüz önce mobil için tasarlanır, sonra büyük ekranlara genişler.
7. **Kod tarafı ayrı (code-behind)**: Her bileşen/sayfa `Component.razor` (işaretleme) + `Component.razor.cs` (`partial class`, iş mantığı) olarak ayrılır. Yalnızca görsel/özyinelemeli `RenderFragment` yardımcıları `.razor`'da küçük bir `@code`'da kalabilir (DESIGN.md §11).
8. **Tailwind interpolasyon yasağı**: `class="@($"px-{x}")"` gibi string ile **Tailwind utility üretmek yasaktır**; sınıf adları tam yazılır (`px-1`). Bileşenlerin içinde tam yazılmış utility'leri serbestçe kullanabilirsin.

---

## 2. Çözüm yapısı

```
turbohesap/
├─ Directory.Build.props        # ortak derleme ayarları (net10.0, nullable, vb.)
├─ Directory.Packages.props     # Central Package Management — tüm sürümler burada
├─ Makefile                     # make run / stop / build / migrate ...
├─ AGENTS.md / DESIGN.md / README.md
└─ src/
   ├─ Turbohesap.Shared/        # Contracts, Cqrs, Services (arayüz), Security, Common
   ├─ Turbohesap.Api/           # Controllers, Features, Services, Entities, Persistence,
   │                            # Mapping, Middleware, Authentication, Configuration
   └─ Turbohesap.Web/           # Blazor: Components, Pages, Services, Models, Frontend
```

### Turbohesap.Shared
| Dizin | İçerik |
|-------|--------|
| `Common/` | `PagedRequest`, `PagedResult<T>`, `ProblemDetail`, `SortDirection` |
| `Cqrs/` | `ICommand<T>`, `IQuery<T>` işaret arayüzleri |
| `Contracts/{Alan}/` | DTO + Command + Query + Validator (her özellik kendi klasöründe; ör. `Diagnostics/WebErrorReport` + `LogWebErrorCommand`) |
| `Services/` | Servis arayüzleri (`ICustomerService`, `IAuthService`, `IWebErrorLogService`) |
| `Security/` | `Roles` (rol adları), `ApiVersions` (sürüm sabitleri) |

### Turbohesap.Api
| Dizin | İçerik |
|-------|--------|
| `Entities/` | EF varlıkları (`BaseEntity`, `Customer`, `User`, `AuditLog`, `ErrorLog`) |
| `Persistence/` | `AppDbContext`, `Configurations/` (entity başına yapılandırma), `Migrations/`, `DbSeeder` |
| `Mapping/` | `MappingRegistry` (giriş noktası) + entity başına `IRegister` dosyaları |
| `Services/` | Servis uygulamaları (transactional iş mantığı; `WebErrorLogService` web hatalarını `error_logs`'a yazar) |
| `Features/{Alan}/` | Wolverine handler'ları + olaylar (events); ör. `Features/Diagnostics/DiagnosticsHandlers` |
| `Controllers/V{n}/` | Sürümlenmiş controller'lar |
| `Middleware/` | `GlobalExceptionMiddleware` |
| `Authentication/` | `JwtOptions`, `JwtTokenService` |
| `Configuration/` | `Program.cs`'i minimal tutan uzantı metotları (her çapraz kesen ayrı dosya) |
| `Common/` | `ICurrentUser`, istisnalar, `PasswordHasher`, `QueryableExtensions` |

### Turbohesap.Web
| Dizin | İçerik |
|-------|--------|
| `Components/Base/` | `TurboComponentBase` (req: tüm bileşenlerin temeli) |
| `Components/{Ad}/` | Yeniden kullanılan bileşenler (Button, Input, Feedback `ThDialog`/`ThToastHost`/`ThErrorBoundary`, Data). Her bileşen **`.razor` + `.razor.cs` + `Th<Ad>.md` (döküman)**; indeks `Components/Components.md` |
| `Components/Shell/` | Sidebar, AppBar, **PageTabs**, CommandLauncher, AppLauncher, **ThemeDesigner**, AiChat |
| `Components/Layout/` | `MainLayout`, `AuthLayout`, `ThPage` (sayfayı PageTabs'a kaydeder, kirli tutamacı cascade eder) |
| `Pages/{Sayfa}/` | Sayfalar (`Index.razor` + `Index.razor.cs` …) |
| `Services/` | HttpClient fabrikası, kimlik, tema, toast, **PageTabService**, navigasyon, **WebErrorReporter** |
| `Models/` | Arayüz modelleri (NavItem, AppItem, ToastMessage, **PageTab**) |
| `Frontend/` | Vite + Tailwind v4 + TypeScript kaynakları (DESIGN.md) |

---

## 3. Teknoloji yığını

| Konu | Kütüphane |
|------|-----------|
| ORM | EF Core 10 + Npgsql + **EFCore.NamingConventions** (snake_case) |
| CQRS / olay veriyolu | **WolverineFx** (+ `WolverineFx.RuntimeCompilation`, `WolverineFx.FluentValidation`) |
| Denetim | **Audit.NET** + `Audit.EntityFramework.Core` |
| Eşleme | **Mapster** + `Mapster.DependencyInjection` |
| Doğrulama | **FluentValidation** (+ Blazor'da `Blazored.FluentValidation`) |
| API dokümanı | **Scalar.AspNetCore** + `Microsoft.AspNetCore.OpenApi` + **Asp.Versioning** |
| Telemetri | **OpenTelemetry** (AspNetCore, Http, Runtime, Npgsql kaynağı) |
| Loglama | **Serilog** (Console + File) |
| Dayanıklılık | **Microsoft.Extensions.Http.Resilience** (Polly) |
| Kimlik | JWT Bearer + PBKDF2 parola özeti |

> Tüm sürümler `Directory.Packages.props`'tadır (Central Package Management). `.csproj` dosyaları sürüm **yazmaz**.

---

## 4. İstek akışı (CQRS)

```
HTTP → Controller → IMessageBus.InvokeAsync(command/query)
                         │  (Wolverine: FluentValidation middleware burada çalışır)
                         ▼
                 [WolverineHandler]  (ince CQRS sınırı)
                         │
                         ▼
                  ServiceInterface → Service   (transactional iş mantığı, AsNoTracking)
                         │                      └─ IMessageBus.PublishAsync(@event) → Event handler
                         ▼
                  EF Core (AppDbContext) → PostgreSQL
                         └─ Audit.NET aynı transaction içinde audit_logs'a yazar
```

**Kurallar:**
- Controller yalnızca `IMessageBus` üzerinden komut/sorgu gönderir; iş mantığı içermez.
- Handler **ince**'dir: mesajı servise delege eder. İş mantığı **her zaman** serviste.
- Wolverine handler sınıfları **static olamaz** (concrete olmalı) ve `[WolverineHandler]` ile işaretlenir.
- EF DbContext ve Mapster IMapper "opak" servis olduğundan `MessagingConfiguration`'da `AlwaysUseServiceLocationFor<AppDbContext>()` ve `<IMapper>()` ile çözülür. Bu altyapıya bağlı yeni bir servis eklersen ek satır gerekmez; tamamen yeni bir opak bağımlılık eklersen oraya bir satır ekle.

---

## 5. Çapraz kesen konular

| Konu | Nerede | Davranış |
|------|--------|----------|
| **Hata yakalama (API)** | `Middleware/GlobalExceptionMiddleware` | Alan istisnaları uygun HTTP koduna çevrilir; 500'ler `error_logs`'a (url, ip, tarih, user-agent, header, satır/dosya, hash, tekrar sayısı) yazılır. Yanıt **daima** `{"detail":"…"}`. |
| **Hata yakalama (Web)** | `ThErrorBoundary` + `WebErrorReporter` + `Controllers/V1/DiagnosticsController` | Blazor'da yakalanan istemci hataları **anonim** `POST api/v1/diagnostics/client-errors` ile aynı `error_logs` tablosuna `Source = "Web"` olarak (hash + tekrar mantığı) yazılır. `WebErrorLogService` kalıcılaştırır; raporlama uygulamayı düşürmez. |
| **Denetim (audit)** | `Configuration/AuditConfiguration` | Audit.NET, EF seviyesinde her değişikliği `audit_logs`'a jsonb olarak ve **aynı transaction içinde** yazar. `AuditLog`/`ErrorLog` `[AuditIgnore]` ile denetlenmez. |
| **Doğrulama** | Shared validator + Wolverine middleware + Blazor | Aynı kural iki tarafta. API'de handler öncesi otomatik çalışır; geçersizse 400 + `{"detail"}`. |
| **Eşleme** | `Mapping/` | Entity başına `IRegister`; `MappingRegistry.AddApplicationMapping()` hepsini tarar. Okumada `ProjectToType<TDto>()` (SQL'e iner). |
| **Sayfalama** | `Common/QueryableExtensions` + `PagedRequest/PagedResult` | Tüm listeler sayfalı; `PageSize` parametrik (üst sınır 200). |
| **Telemetri** | `Configuration/TelemetryConfiguration` | ActivitySource `Turbohesap.Api`; OTLP yalnızca `Telemetry:OtlpEndpoint` doluysa. |
| **Loglama** | `Configuration/SerilogConfiguration` + appsettings | Console + günlük dosya. |
| **Kimlik/rol** | `Authentication/` + `Security/Roles` | JWT; roller `Roles` sabitlerinde; controller'da `[Authorize(Roles = ...)]`. |
| **Sürümleme** | `Configuration/ApiDocumentationConfiguration` | `api/v{version}/...`; Scalar'da her uç noktanın erişebildiği roller otomatik yazılır. |
| **snake_case** | `PersistenceConfiguration` | `UseSnakeCaseNamingConvention()` ile tüm tablo/kolon adları snake_case. |

---

## 6. ✅ Yeni bir varlık ekleme (uçtan uca reçete)

Örnek: **Tedarikçi (Supplier)** ekleyelim. Sıra önemlidir.

### 6.1 Shared — sözleşmeler
1. **DTO** → `Shared/Contracts/Suppliers/SupplierDto.cs`
2. **Komutlar** → `CreateSupplierCommand : ICommand<SupplierDto>`, `UpdateSupplierCommand`, `DeleteSupplierCommand : ICommand<bool>`
3. **Sorgular** → `GetSupplierByIdQuery : IQuery<SupplierDto?>`, `GetSuppliersQuery : PagedRequest, IQuery<PagedResult<SupplierDto>>`
4. **Doğrulayıcılar** → `CreateSupplierCommandValidator`, `UpdateSupplierCommandValidator` (FluentValidation, Türkçe mesajlar)
5. **Servis arayüzü** → `Shared/Services/ISupplierService.cs`

### 6.2 Api — uygulama
6. **Entity** → `Api/Entities/Supplier.cs : BaseEntity`
7. **EF yapılandırma** → `Api/Persistence/Configurations/SupplierConfiguration.cs` (`ToTable("suppliers")`, indeksler, alan uzunlukları)
8. **Mapping** → `Api/Mapping/SupplierMappingConfig.cs : IRegister` (Entity↔DTO, Command→Entity, denetim alanlarını `Ignore`)
9. **Servis** → `Api/Services/SupplierService.cs : ISupplierService` (transactional yazma, `AsNoTracking` + `ProjectToType` okuma, `IMessageBus.PublishAsync` olay)
10. **Handler** → `Api/Features/Suppliers/SupplierHandlers.cs` (`[WolverineHandler]`, her mesaj için `Handle` → servise delege)
11. **Olaylar** → `Api/Features/Suppliers/SupplierEvents.cs` (`SupplierCreatedEvent` + `[WolverineHandler]` dinleyici)
12. **DI** → `Configuration/ApplicationServicesConfiguration`'a `AddScoped<ISupplierService, SupplierService>()`
13. **Controller** → `Api/Controllers/V1/SuppliersController.cs : ApiControllerBase` (`[ApiVersion]`, `[Authorize(Roles = ...)]`, `IMessageBus.InvokeAsync`)
14. **Migration** → `dotnet ef migrations add AddSupplier --project src/Turbohesap.Api -o Persistence/Migrations` → `make migrate`

### 6.3 Web — arayüz
15. **API servisi** → `Web/Services/SupplierApiService.cs` (`ApiClient` üzerinden, sorgu string'i kurar) + Program.cs'te `AddScoped`
16. **Sayfa** → `Web/Pages/Suppliers/Index.razor` **+ `Index.razor.cs`** (`@page "/suppliers"`, `[Authorize]`, `ThPage` + `ThDataTable` + `ThPagination` + `ThDialog` form). `ThPage`'e `TabIcon` ver; sayfa render olunca **kendini PageTabs'a kaydeder** (ekstra kod gerekmez). Düzenleme formunda `EditContext.OnFieldChanged` → `[CascadingParameter] PageTabHandle.MarkDirty()`, kaydet/iptal sonrası `MarkClean()` (kirli sekme kapatma uyarısı için).
17. **Menü** → `Web/Services/AppNavigation.cs`'e `NavItem { Label="Tedarikçiler", Href="/suppliers", Roles=[...] }`. Menü tıklaması yalnızca gezinir; sekme açmaz (bulunamayan sayfalar sekme oluşturmaz).

Referans uygulama: **Customer** (yukarıdaki her adım `Customer*` dosyalarında mevcuttur).

---

## 7. Sözleşmeler & kurallar

- **Central Package Management**: yeni paket → `Directory.Packages.props`'a `<PackageVersion>`, `.csproj`'a sürümsüz `<PackageReference>`.
- **Program.cs minimal**: yeni çapraz kesen konu → `Configuration/` altında `AddXxx`/`UseXxx` uzantısı.
- **Sayfa yolları** API gibidir ama `/api/{version}` öneki yoktur: API `api/v1/customers` ↔ Web `/customers`.
- **Roller** yalnızca `Roles` sabitlerinden gelir; string yazılmaz.
- **Listeleme** her zaman `PagedRequest`/`PagedResult` ile.
- **Frontend sınıf adları açıkça yazılır**; `$"p-{x}"` türü interpolasyon yasaktır (DESIGN.md §Tailwind).

---

## 8. Komutlar

```bash
make help        # tüm komutlar
make install     # restore + npm install
make build       # frontend + dotnet build
make migrate     # EF migration uygula
make run         # frontend derle + API & Web başlat (URL'leri yazar)
make stop        # hepsini durdur
make restart     # durdur + başlat
make status      # çalışma durumu
make logs        # logları izle
make api / web   # tek projeyi ön planda çalıştır
make front-watch # frontend'i izleyerek derle
```

Yerel: API `:5080`, Web `:5180`, PostgreSQL `turbohesap`. Demo: `admin@turbohesap.local / Admin123!`.

---

## 9. Bileşen dökümantasyonu & üretim becerileri (skills)

Her arayüz bileşeni **kendi dizininde** bir `Th<Ad>.md` dökümanı taşır (amaç, parametre tablosu,
varyant/boyutlar, kullanılan `--th-*` token'ları, kullanım örneği, tema/uyum notları). Tüm
bileşenlerin indeksi `src/Turbohesap.Web/Components/Components.md`'dedir (`create-component` /
`update-component` tarafından güncel tutulur).

**Beceriler** (`.claude/skills/`) — yeni/var olan bileşenleri sistemle %100 tutarlı üretir/günceller:

| Beceri | Ne zaman | Ne yapar |
|--------|----------|----------|
| **create-component** | Sıfırdan yeni bir `Th<Ad>` bileşeni | `.razor` + `.razor.cs` + `Frontend/css/<ad>.css` (+ `main.css` import) + gerekirse TS interop (`scripts/<ad>.ts` + `interop.ts` + `Services/<Ad>Interop.cs` + DI) + `Th<Ad>.md` + `Components.md` satırı; sonunda `dotnet build` + `npm run build` ile doğrular. Yeni klasörse `_Imports.razor`'a `@using` ekler. |
| **update-component** | Var olan bir bileşeni değiştirmek **veya dökümanını ilk kez yazmak** | İlgili dosyaları kuralları bozmadan günceller; imza değişirse tüm kullanım yerlerini düzeltir; `Th<Ad>.md`'yi yazar/günceller; `Components.md`'yi tazeler; derleyerek doğrular. |

**Bileşen değişmezleri** (her iki beceri ve elle yazımda geçerli):
1. **Code-behind ayrımı** zorunlu (§1.7). Mantık asla `.razor`'da değil.
2. **Token-first CSS**: yalnız `--th-*`; sabit renk/gölge/yarıçap yok. Renk→`--th-*`, köşe→`--th-radius-*`, gölge→`--th-card-shadow`/`--th-shadow-*`, ritim→`--th-nav-pad-y`/`--th-row-pad-y`/`--th-density-gap`, hareket→`--th-transition*`/`--th-ease`, katman→`--th-z-*`. Yazı tipi gövdeden miras (mono hariç). Böylece bileşen renk/tema/yoğunluk/yarıçap/gölge/font değişimine **otomatik** uyar.
3. **Tailwind serbest, interpolasyon yasak** (§1.8). Varyant/boyut C# `switch` → sabit `th-<ad>--*` (`TurboComponentBase.SizeClass`/`Cx`).
4. **Yüksek performans / yoğun DOM** gereken bileşenlerde TypeScript yazılıp `window.turbohesap` + bir `*Interop` servisiyle C#'a bağlanır; aksi halde TS yazılmaz.
