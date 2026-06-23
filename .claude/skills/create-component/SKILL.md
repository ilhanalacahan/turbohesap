---
name: create-component
description: Turbohesap Blazor tasarım sistemi için sıfırdan yeni, eksiksiz bir bileşen oluşturur. Tema/token sistemiyle %100 uyumlu, code-behind ayrımıyla, gerekirse TypeScript interop ile, kendi CSS'i + {Component}.md dökümanı ve Components.md kaydıyla. Kullanıcı "yeni bileşen", "create component", "bir X bileşeni ekle/yaz" dediğinde kullan.
allowed-tools: Read, Write, Edit, Bash, Grep, Glob
---

# create-component — Turbohesap bileşeni oluşturucu

Bu beceri, **DESIGN.md** ve **AGENTS.md**'deki kuralların tek kaynak olduğu, hata payı bırakmayan bir reçeteyle yeni bir Blazor bileşeni üretir. Şaşma. Her adımı uygula, sonunda derleyerek doğrula.

## 0. Önce oku (zorunlu)
1. `DESIGN.md` — token sistemi, Tailwind entegrasyonu, bileşen kataloğu.
2. `AGENTS.md` §1 (ilkeler), §2 (Web dizinleri).
3. `src/Turbohesap.Web/Components/Base/TurboComponentBase.cs` — `Class`, `Style`, `AdditionalAttributes`, `Cx(...)`, `SizeClass(...)`.
4. En yakın referans bileşeni oku ve örnek al: varyant/boyut için **ThButton**, generic + yuvalar için **ThDataTable**, overlay/footer için **ThDialog**, JS gerektiren için **ThemeInterop + scripts/theme.ts**.

## 1. Değişmez kurallar (asla ihlal etme)
- **Code-behind ayrımı**: `Th<Ad>.razor` (yalnız işaretleme + direktifler) **+** `Th<Ad>.razor.cs` (`public partial class Th<Ad>`, tüm C#). Yalnız özyinelemeli/görsel `RenderFragment` yardımcıları `.razor`'da küçük bir `@code`'da kalabilir.
- **Token-first CSS**: `Frontend/css/<ad>.css` içinde **yalnızca `--th-*` token'ları** kullan. Sabit renk (hex/rgb), sabit gölge, sabit yarıçap **yasak**. Sabit `px` yalnız 1px kenarlık gibi gerçekten sabit mikro ölçüler için; boyutlar `rem`.
- **Tema duyarlılığı**: renk→`--th-*`, köşe→`--th-radius-*`, gölge→`--th-card-shadow` (kart/yüzey) veya `--th-shadow-*` (açılır/menü), dikey ritim→`--th-nav-pad-y`/`--th-row-pad-y`/`--th-density-gap`, kontrol yüksekliği→`--th-control-height*`, hareket→`--th-transition*`+`--th-ease`, katman→`--th-z-*`. Yazı tipini **set etme** (gövdeden miras alır); yalnız mono gerekiyorsa `--th-font-mono`. Böylece bileşen renk/tema/yoğunluk/yarıçap/gölge/font değişimlerine otomatik uyar.
- **Tailwind serbest, interpolasyon yasak**: `.razor` işaretlemesinde Tailwind utility'lerini **tam yazarak** serbestçe kullan (`flex items-center gap-2`, `bg-primary`, `text-text-muted`, `rounded-md`, `shadow-lg` — hepsi `@theme inline` ile token'a bağlı). **Asla** `class="@($"px-{x}")"`, `"p-"+n`, string birleştirme ile Tailwind sınıfı üretme. Varyant/boyutu C# `switch` → **sabit** `th-<ad>--<varyant>` sınıfına eşle (`SizeClass`/`Cx` kullan).
- **Açık temada koyu temayı da düşün**: token'lara bağlı kaldıysan otomatik çalışır; sabit değer yazmadığını doğrula.

## 2. Adımlar

### 2.1 İsim ve yer
- Bileşen adı PascalCase, `Th` önekiyle: `Th<Ad>` (ör. `ThTabs`, `ThTooltip`).
- Klasör: `src/Turbohesap.Web/Components/<Ad>/` (mevcut bir aileye aitse o klasör: `Button/`, `Input/`, `Feedback/`, `Data/`, `Shell/`, `Layout/`).
- Namespace: `Turbohesap.Web.Components.<Ad>`.
- CSS taban sınıfı: `th-<ad>` (kebab-case), varyant `th-<ad>--<varyant>`, boyut `th-<ad>--sm|md|lg`.

### 2.2 `Th<Ad>.razor` (işaretleme)
```razor
@namespace Turbohesap.Web.Components.<Ad>
@inherits Turbohesap.Web.Components.Base.TurboComponentBase
@* Kod tarafı: Th<Ad>.razor.cs *@

<div class="@RootClass" style="@Style" @attributes="AdditionalAttributes">
    @ChildContent
</div>
```
Notlar: kök öğeye daima `Class`/`Style`/`AdditionalAttributes`'ı geçir (DOM esnekliği). Yuva gerekiyorsa `RenderFragment` parametreleri kullan (`ChildContent`, `Header`, `Footer`...). Generic ise `.razor`'a `@typeparam T` ekle ve `.cs`'te `partial class Th<Ad><T>` yaz.

### 2.3 `Th<Ad>.razor.cs` (kod tarafı)
```csharp
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.<Ad>;

/// <summary>Bir cümlelik amaç. th-<ad> sarmalayıcısı.</summary>
public partial class Th<Ad>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public <Ad>Variant Variant { get; set; } = <Ad>Variant.Default;
    [Parameter] public EventCallback<Microsoft.AspNetCore.Components.Web.MouseEventArgs> OnClick { get; set; }

    private string RootClass => Cx(
        "th-<ad>",
        VariantClass,
        SizeClass(Size, "th-<ad>"),
        Class);

    private string VariantClass => Variant switch
    {
        <Ad>Variant.Primary => "th-<ad>--primary",
        _ => "th-<ad>--default"
    };
}
```
- `@inject` ile gelen servisleri `.razor.cs`'ten doğrudan kullanabilirsin (üretilen property'ler) ya da `[Inject]` ekle.
- Varyant/boyut **daima** sabit sınıf eşlemesiyle; enum dosyası gerekiyorsa `Components/<Ad>/<Ad>Variant.cs`.

### 2.4 `Frontend/css/<ad>.css` (stil)
- Yalnız `--th-*` token'ları. Örnek iskelet:
```css
/* <Ad> — th-<ad>. Yalnızca --th-* token'ları kullanır. */
.th-<ad> {
  display: flex;
  gap: var(--th-density-gap);
  padding: var(--th-nav-pad-y) var(--th-content-padding);
  background: var(--th-surface);
  color: var(--th-text);
  border: 1px solid var(--th-border);
  border-radius: var(--th-radius-lg);
  box-shadow: var(--th-card-shadow);
  transition: background-color var(--th-transition-fast) var(--th-ease);
}
.th-<ad>--primary { background: var(--th-primary); color: var(--th-primary-fg); }
.th-<ad>--sm { /* küçük boyut */ }
.th-<ad>--lg { /* büyük boyut */ }
```
- **Kaydet**: `Frontend/main.css`'in bileşen `@import` bloğuna ekle:
  `@import './css/<ad>.css' layer(components);`

### 2.5 Namespace kaydı (KRİTİK — atlanırsa derlenir ama bileşen markup'ta bulunamaz)
- Yeni klasör açtıysan `src/Turbohesap.Web/_Imports.razor`'a `@using Turbohesap.Web.Components.<Ad>` satırını ekle (mevcut `@using ...Components.*` listesinin yanına).

### 2.6 (Gerekliyse) Yüksek performans / yoğun DOM → TypeScript interop
Sadece çok sık/karmaşık DOM işlemi gerektiren bileşenlerde (sürükle-bırak, sanal liste, ölçüm, gözlemci, animasyon zamanlayıcı):
1. `Frontend/scripts/<ad>.ts` yaz; saf tarayıcı işini burada yap, `dispose` döndür.
2. `Frontend/scripts/interop.ts` içinde `window.turbohesap` API'sine ekle (mevcut `theme`/`focusTrap` gibi).
3. C# köprüsü: `src/Turbohesap.Web/Services/<Ad>Interop.cs` (bkz. `ThemeInterop` deseni; `IJSRuntime` enjekte, `InvokeVoidAsync`/`InvokeAsync`).
4. DI: `src/Turbohesap.Web/Program.cs`'e `builder.Services.AddScoped<<Ad>Interop>();`.
5. Bileşen `IAsyncDisposable` ile JS kaynağını bırakır.
Aksi halde **TypeScript yazma**; Blazor Interactive Server etkileşimi C#'ta yönetir.

### 2.7 `Th<Ad>.md` (döküman — bileşenle AYNI dizinde)
`Components/<Ad>/Th<Ad>.md` oluştur (şablon §3).

### 2.8 `Components/Components.md` kaydı
Kök indekse bir satır ekle (§4).

### 2.9 Doğrula (zorunlu)
```bash
dotnet build src/Turbohesap.Web/Turbohesap.Web.csproj -nologo -clp:NoSummary
( cd src/Turbohesap.Web/Frontend && npm run build )
```
İkisi de **Build succeeded / built** demeden işi bitmiş sayma. Hata varsa düzelt.

## 3. `Th<Ad>.md` döküman şablonu
```markdown
# Th<Ad>

> Bir cümlelik amaç.

- **Sınıf**: `th-<ad>` (+ `--<varyant>`, `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.<Ad>`
- **Dosyalar**: `Th<Ad>.razor`, `Th<Ad>.razor.cs`, `Frontend/css/<ad>.css`[, `Frontend/scripts/<ad>.ts` + `Services/<Ad>Interop.cs`]

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | İçerik |
| `Variant` | `<Ad>Variant` | `Default` | Görsel varyant |
| `Size` | `ComponentSize` | `Md` | Boyut |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | Tıklama |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Kullanım
\`\`\`razor
<Th<Ad> Variant="<Ad>Variant.Primary" Size="ComponentSize.Lg">İçerik</Th<Ad>>
\`\`\`

## Kullanılan token'lar
`--th-surface`, `--th-text`, `--th-border`, `--th-radius-lg`, `--th-card-shadow`, `--th-density-gap` … (gerçekten kullandıkların).

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda doğrulandı.
```

## 4. `Components.md`'ye eklenecek satır
`Components/Components.md` içindeki tabloya:
`| Th<Ad> | [<Ad>/Th<Ad>.md](<Ad>/Th<Ad>.md) | Bir cümlelik özet | ✅ |`

## 5. Bitiş kontrol listesi
- [ ] `.razor` + `.razor.cs` ayrı, namespace eşleşiyor
- [ ] CSS yalnız `--th-*`; `main.css`'e `@import` eklendi
- [ ] Yeni klasörse `_Imports.razor`'a `@using` eklendi
- [ ] Tailwind utility'leri tam yazıldı, interpolasyon yok
- [ ] (Varsa) TS interop + Interop servisi + DI + IAsyncDisposable
- [ ] `Th<Ad>.md` yazıldı, `Components.md`'ye satır eklendi
- [ ] `dotnet build` + `npm run build` başarılı
