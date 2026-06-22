# ThSpinner

> Operasyonların ve arka plan işlemlerinin devam ettiğini belirten, dairesel yükleniyor (spinner) animasyonu bileşeni.

- **Sınıf**: `th-spinner` (+ `--sm|lg`, `--success|warning|danger|info|neutral`)
- **Namespace**: `Turbohesap.Web.Components.Spinner`
- **Dosyalar**: `ThSpinner.razor`, `ThSpinner.razor.cs`, `Frontend/css/spinner.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Size` | `ComponentSize` | `Md` | Çap boyutu (`Sm`, `Md`, `Lg`) |
| `Variant` | `SpinnerVariant` | `Primary` | Durum renk varyasyonu (`Primary`, `Success`, `Warning`, `Danger`, `Info`, `Neutral`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThSpinner Size="ComponentSize.Sm" Variant="SpinnerVariant.Success" />
<ThSpinner Size="ComponentSize.Lg" />
```

## Kullanılan token'lar
- `--th-border`
- `--th-primary`
- `--th-success`
- `--th-warning`
- `--th-danger`
- `--th-info`
- `--th-text`
- `--th-radius-full`

## Tema/uyum notları
- **Erişilebilirlik**: `role="status"` niteliği ve iç içe geçmiş ekran okuyucu dostu `sr-only` etiketiyle donatılmıştır.
- **Koyu Tema**: Dairesel çerçeve arka plan rengi aktif temaya uyumlu transparanlıklar ve kenarlık tonları barındırır.
