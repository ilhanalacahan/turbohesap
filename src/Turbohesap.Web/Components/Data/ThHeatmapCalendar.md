# ThHeatmapCalendar

> Katkı/Yoğunluk matrisi stilinde, gün bazında aktivite sıklığını görselleştiren takvim bileşeni.

- **Sınıf**: `th-heatmap-calendar` (+ `--primary`, `--success`, `--warning`, `--danger`)
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThHeatmapCalendar.razor`, `ThHeatmapCalendar.razor.cs`, `Frontend/css/heatmapcalendar.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Title` | `string` | `"Aktivite Yoğunluk Matrisi"` | Panel başlığı |
| `ActivityData` | `Dictionary<DateTime, int>?` | — | Tarih ve aktivite sıklığını barındıran veri kümesi |
| `MaxActivityValue` | `int` | `10` | En yoğun renk düzeyi (seviye 4) için kabul edilen eşik değer |
| `ColorVariant` | `HeatmapColor` | `Primary` | Renk paleti varyantı |
| `MonthCount` | `int` | `3` | Geriye doğru kaç aylık matris çizileceği |

## Kullanım
```razor
<ThHeatmapCalendar Title="Depo Sevkiyat Yoğunluğu"
                   ActivityData="@_shipmentActivities"
                   MaxActivityValue="8"
                   ColorVariant="HeatmapColor.Success"
                   MonthCount="4" />
```

## Kullanılan token'lar
`--th-surface`, `--th-border`, `--th-radius-lg`, `--th-card-shadow`, `--th-text-muted`, `--th-text-subtle`, `--th-primary`, `--th-primary-subtle`, `--th-success`, `--th-success-subtle`, `--th-warning`, `--th-warning-subtle`, `--th-danger`, `--th-danger-subtle`.

## Tema/uyum notları
Seçilen renk varyantına göre `color-mix` yöntemiyle dinamik renk kademeleri oluşturur. Koyu/açık tema değişimlerine otomatik uyum sağlar.
