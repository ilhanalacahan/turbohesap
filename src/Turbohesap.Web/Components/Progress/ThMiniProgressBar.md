# ThMiniProgressBar

> Tablo satırlarına ve küçük alanlara sığacak boyutta çoklu kategorik ilerleme çubuğu ve göstergesi.

- **Sınıf**: `th-mini-progress-bar` (+ `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Progress`
- **Dosyalar**: `ThMiniProgressBar.razor`, `ThMiniProgressBar.razor.cs`, `Frontend/css/miniprogressbar.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Segments` | `List<ProgressSegment>` | `new()` | İlerleme çubuğunda gösterilecek dilimlerin verisi |
| `Size` | `ComponentSize` | `Md` | İlerleme barının yüksekliği (`Sm`: 5px, `Md`: 8px, `Lg`: 12px) |
| `ShowLegend` | `bool` | `true` | Alt gösterge (kategori etiketleri) listesinin gösterilip gösterilmeyeceği |

### ProgressSegment Modeli
- `Label` (`string`): Kategori adı (Örn: "Alışlar").
- `Value` (`double`): Kategoriye ait değer (Oransal hesaplama için kullanılır).
- `ColorClass` (`string`): Tailwind arka plan rengi sınıfı (Örn: `bg-primary`, `bg-success`, `bg-warning`, `bg-danger`).

## Kullanım
```razor
<ThMiniProgressBar Segments="@_budgetSegments" 
                   Size="ComponentSize.Md" 
                   ShowLegend="true" />
```

## Kullanılan token'lar
`--th-surface-3`, `--th-radius-full`, `--th-text-subtle`, `--th-text-muted`, `--th-transition-normal`.

## Tema/uyum notları
Yumuşak CSS bar büyüme geçişlerine sahiptir. Koyu ve açık modlarla tam uyumludur.
