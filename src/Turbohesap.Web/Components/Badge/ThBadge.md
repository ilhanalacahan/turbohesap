# ThBadge

> Durum belirten renkler, boyut varyasyonları ve isteğe bağlı nokta göstergesiyle tasarlanmış rozet/etiket bileşeni.

- **Sınıf**: `th-badge` (+ `--sm|lg`, `--neutral|primary|success|warning|danger|info`), `th-badge__dot`
- **Namespace**: `Turbohesap.Web.Components.Badge`
- **Dosyalar**: `ThBadge.razor`, `ThBadge.razor.cs`, `Frontend/css/badge.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | Rozetin içindeki metin veya elementler |
| `Variant` | `BadgeVariant` | `Neutral` | Görsel durum rengi (`Neutral`, `Primary`, `Success`, `Warning`, `Danger`, `Info`) |
| `ShowDot` | `bool` | `false` | Sol tarafa yuvarlak bir durum noktası ekler |
| `Size` | `ComponentSize` | `Md` | Rozet boyutu (`Sm`, `Md`, `Lg`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThBadge Variant="BadgeVariant.Success" ShowDot="true">Aktif</ThBadge>
<ThBadge Variant="BadgeVariant.Danger">Pasif</ThBadge>
```

## Kullanılan token'lar
- `--th-primary`
- `--th-primary-subtle`
- `--th-success`
- `--th-success-subtle`
- `--th-warning`
- `--th-warning-subtle`
- `--th-danger`
- `--th-danger-subtle`
- `--th-info`
- `--th-info-subtle`
- `--th-surface-3`
- `--th-text-muted`
- `--th-radius-full`

## Tema/uyum notları
- **Dinamik Renkler**: Arka plan ve yazı renkleri, aktif temanın soft durum renk varyantlarından (`-subtle`) türetilir. Koyu temada gözü yormayan opak zeminlere dönüşür.
