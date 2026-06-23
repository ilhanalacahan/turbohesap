# ThSegmentedControl

> Düğme görünümlü, yan yana hizalanan seçenekler barındıran ve aktif seçeneği kayan efektle gösteren estetik giriş (segmented control) bileşeni.

- **Sınıf**: `th-segmented-control` (+ `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThSegmentedControl.razor`, `ThSegmentedControl.razor.cs`, `Frontend/css/segmentedcontrol.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `TValue` | — | Seçilen aktif değer |
| `ValueChanged` | `EventCallback<TValue>` | — | Değer değiştiğinde tetiklenen olay |
| `ValueExpression` | `Expression<Func<TValue>>` | — | Blazor form binding için ifade |
| `Options` | `Dictionary<TValue, string>` | — | Seçenek listesi (Key: Değer, Value: Ekranda gösterilecek metin) |
| `Disabled` | `bool` | `false` | Girişin pasif olma durumu |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Kullanım
```razor
<ThSegmentedControl TValue="string" 
                    @bind-Value="_activeFilter" 
                    Options="@(new() { { "ALL", "Tümü" }, { "IN", "Giriş" }, { "OUT", "Çıkış" } })" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-border-strong`, `--th-text`, `--th-text-muted`, `--th-primary`, `--th-danger`, `--th-shadow-xs`, `--th-radius-full`, `--th-transition`, `--th-transition-fast`, `--th-ease`, `--th-fw-medium`, `--th-fw-semibold`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Seçenekler arası geçiş yaparken active slider CSS transform/left geçişini (`--th-transition`) kullanarak pürüzsüz bir kayma animasyonu sunar.
