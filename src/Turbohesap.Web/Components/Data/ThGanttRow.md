# ThGanttRow

> Mini süreç planlaması, makine zaman çizelgesi ve üretim sipariş takibi için kullanılan mini Gantt şeridi bileşeni.

- **Sınıf**: `th-gantt-row` (+ `--sm|md|lg`, `--completed|active|delayed|pending`)
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThGanttRow.razor`, `ThGanttRow.razor.cs`, `Frontend/css/ganttrow.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Label` | `string` | `"İş Süreci"` | Gantt satırının solunda gösterilen ana başlık |
| `Subtitle` | `string` | — | İkincil alt başlık |
| `Segments` | `List<GanttSegment>` | — | Gantt şeridi üzerindeki süreç dilimleri listesi |
| `ShowTicks` | `bool` | `true` | Zaman/yüzde kılavuz çizgilerinin (ticks) gösterilme durumu |
| `Size` | `ComponentSize` | `Md` | Şerit yüksekliği varyantı (`Sm`, `Md`, `Lg`) |

### GanttSegment Sınıfı
- `Label` (`string`): Segmentin adı (Ör: `"Kesim"`)
- `StartPercent` (`double`): Başlangıç yüzde konumu (0 - 100)
- `EndPercent` (`double`): Bitiş yüzde konumu (0 - 100)
- `Status` (`GanttSegmentStatus`): Segmentin durum değeri (`Completed`, `Active`, `Delayed`, `Pending`)
- `Tooltip` (`string?`): Detaylı açıklama hover tooltip metni
- `Icon` (`string?`): FontAwesome ikon sınıf adı (Ör: `"fa-solid fa-check"`)

## Kullanım
```razor
<ThGanttRow Label="İş Emri #2045"
            Subtitle="CNC Freze İstasyonu"
            Segments="@(new() {
                new() { Label = "Hazırlık", StartPercent = 0, EndPercent = 15, Status = GanttSegmentStatus.Completed, Icon = "fa-solid fa-gears" },
                new() { Label = "Kesim", StartPercent = 15, EndPercent = 50, Status = GanttSegmentStatus.Completed, Tooltip = "15:00 - 17:30 arası tamamlandı." },
                new() { Label = "Büküm", StartPercent = 50, EndPercent = 80, Status = GanttSegmentStatus.Active, Tooltip = "Şu an işleniyor." },
                new() { Label = "Montaj", StartPercent = 80, EndPercent = 100, Status = GanttSegmentStatus.Pending }
            })" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-surface-3`, `--th-border`, `--th-border-strong`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-text-inverted`, `--th-success`, `--th-warning`, `--th-danger`, `--th-shadow-xs`, `--th-shadow-sm`, `--th-shadow-lg`, `--th-radius-sm`, `--th-radius-md`, `--th-radius-full`, `--th-transition-fast`, `--th-ease`, `--th-fw-semibold`, `--th-fw-bold`, `--th-content-padding`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık ve koyu temalarda test edilmiş ve tam uyumluluk sağlanmıştır.
