# ThCapacityGauge

> SVG tabanlı, dairesel kapasite ve doluluk oranını gösteren estetik kadran bileşeni.

- **Sınıf**: `th-capacity-gauge` (+ `--sm|md|lg`, `--success|warning|danger`)
- **Namespace**: `Turbohesap.Web.Components.Gauge`
- **Dosyalar**: `ThCapacityGauge.razor`, `ThCapacityGauge.razor.cs`, `Frontend/css/capacitygauge.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `double` | `0` | Mevcut dolu/kullanılan miktar |
| `Max` | `double` | `100` | Toplam kapasite |
| `Unit` | `string` | `"%"` | Değer birimi (Ör: `Palet`, `kg`, `m³`) |
| `Label` | `string` | `"Kapasite"` | Ana başlık |
| `Subtitle` | `string` | — | İkincil alt başlık |
| `ShowDetails` | `bool` | `true` | Alt kısımdaki sayısal detayların gösterilme durumu |
| `ThresholdWarning` | `double` | `75` | Uyarı sınır yüzdesi (Sarı renk için) |
| `ThresholdDanger` | `double` | `90` | Tehlike/kritik sınır yüzdesi (Kırmızı renk için) |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |

## Kullanım
```razor
<ThCapacityGauge Value="150" 
                 Max="200" 
                 Unit="Palet" 
                 Label="A Bölgesi Raf Doluluğu" 
                 ThresholdWarning="80" 
                 ThresholdDanger="95" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-3`, `--th-border`, `--th-card-shadow`, `--th-shadow-md`, `--th-shadow-sm`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-success`, `--th-success-subtle`, `--th-warning`, `--th-warning-subtle`, `--th-danger`, `--th-danger-subtle`, `--th-radius-lg`, `--th-radius-full`, `--th-transition`, `--th-transition-slow`, `--th-ease`, `--th-fw-medium`, `--th-fw-bold`, `--th-content-padding`, `--th-density-gap`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık ve koyu temalarda test edilmiş ve tam uyumluluk sağlanmıştır.
