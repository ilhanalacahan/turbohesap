# ThTooltip

> Hover veya Focus durumunda açılan, hafif ve yüksek performanslı bilgilendirme kutusu (tooltip) bileşeni.

- **Sınıf**: `th-tooltip-wrap`, `th-tooltip` (+ `--top|bottom|left|right`)
- **Namespace**: `Turbohesap.Web.Components.Tooltip`
- **Dosyalar**: `ThTooltip.razor`, `ThTooltip.razor.cs`, `Frontend/css/tooltip.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | Hover/focus tetikleyici hedef element |
| `Text` | `string` | `""` | Gösterilecek ipucu metni |
| `Position` | `TooltipPosition` | `Top` | Açılış yönü (`Top`, `Bottom`, `Left`, `Right`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThTooltip Text="Bilgileri Güncelle" Position="TooltipPosition.Top">
    <ThButton Variant="ButtonVariant.Ghost" Icon="fa-solid fa-sync" />
</ThTooltip>
```

## Kullanılan token'lar
- `--th-sidebar-bg`
- `--th-sidebar-fg`
- `--th-radius-sm`
- `--th-shadow-md`
- `--th-transition-fast`
- `--th-ease`
- `--th-z-tooltip`

## Tema/uyum notları
- **CSS-Only**: C# yaşam döngüsü gecikmelerinden etkilenmez, saf CSS animasyonları ve koordinat dönüşümleriyle çalışır.
- **Koyu Tema**: Sabit renk kodu içermez; `--th-sidebar-bg` ve `--th-sidebar-fg` tokenlarını kullanarak karanlık temada da belirgin kalır.
