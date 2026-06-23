# ThProgress

> Durum bildiren bar renkleri, boyut seçenekleri, etiket ve sayısal yüzde göstergesiyle donatılmış, erişilebilir bir ilerleme çubuğu (progress bar) bileşeni.

- **Sınıf**: `th-progress-wrap` (+ `--sm|lg`, `--success|warning|danger|info`), `th-progress-header`, `th-progress`, `th-progress__bar`
- **Namespace**: `Turbohesap.Web.Components.Progress`
- **Dosyalar**: `ThProgress.razor`, `ThProgress.razor.cs`, `Frontend/css/progress.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `double` | `0` | Mevcut ilerleme değeri |
| `Max` | `double` | `100` | Hedef maksimum değer |
| `Label` | `string?` | — | Sol üst köşede gösterilecek başlık etiketi |
| `ShowValue` | `bool` | `false` | Sağ üst köşede yüzdeyi (Örn. "%45") gösterir |
| `Size` | `ComponentSize` | `Md` | İlerleme çubuğunun yüksekliği (`Sm`, `Md`, `Lg`) |
| `Variant` | `ProgressVariant` | `Primary` | Çubuğun durum rengi (`Primary`, `Success`, `Warning`, `Danger`, `Info`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThProgress Value="75" Max="100" Label="Yükleme Durumu" ShowValue="true" Variant="ProgressVariant.Success" />
```

## Kullanılan token'lar
- `--th-primary`
- `--th-success`
- `--th-warning`
- `--th-danger`
- `--th-info`
- `--th-surface-3`
- `--th-text-muted`
- `--th-radius-full`
- `--th-transition`
- `--th-ease`
- `--th-font-sans`

## Tema/uyum notları
- **Erişilebilirlik**: `progressbar` rolü ve `aria-valuenow`, `aria-valuemin`, `aria-valuemax` nitelikleriyle donatılmıştır.
- **Kritik Uyum**: Genişlik (`width`) hesabı, Türkçe vb. bölgesel ayarlardan etkilenmemesi için `System.Globalization.CultureInfo.InvariantCulture` ile CSS standartlarına (noktalı ondalık ayracı) tam uyumlu biçimde üretilir.
