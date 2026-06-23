# ThStockAlertLevel

> Stok seviyesini minimum ve maksimum limitlerle görselleştirerek kritik, normal ve fazla stok bölgelerini karşılaştıran görsel indikatör.

- **Sınıf**: `th-stock-alert-level` (+ `--sm|md|lg`, `--critical|normal|excess`)
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThStockAlertLevel.razor`, `ThStockAlertLevel.razor.cs`, `Frontend/css/stockalertlevel.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `double` | `0` | Mevcut stok miktarı |
| `MinLimit` | `double` | `0` | Minimum kritik limit |
| `MaxLimit` | `double` | `0` | Maksimum fazla stok limiti |
| `Unit` | `string` | `"Adet"` | Miktar birimi (Ör: `Adet`, `kg`, `Kutu`) |
| `Label` | `string` | `"Stok Seviyesi"` | Gösterge başlığı |
| `ShowLabels` | `bool` | `true` | Başlık, badge ve limit yazılarının gösterilme durumu |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |

## Kullanım
```razor
<ThStockAlertLevel Value="45" 
                   MinLimit="20" 
                   MaxLimit="150" 
                   Unit="Adet" 
                   Label="Çelik Vida Stoğu" />
```

## Kullanılan token'lar
`--th-surface`, `--th-border`, `--th-border-strong`, `--th-text`, `--th-text-muted`, `--th-primary`, `--th-success`, `--th-warning`, `--th-danger`, `--th-success-subtle`, `--th-warning-subtle`, `--th-danger-subtle`, `--th-radius-sm`, `--th-radius-md`, `--th-radius-full`, `--th-shadow-sm`, `--th-transition-fast`, `--th-ease`, `--th-fw-semibold`, `--th-density-gap`.

## Tema/uyum notları
Renk/yarıçap/yoğunluk/gölge token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık ve koyu temalarda test edilmiş ve tam uyumluluk sağlanmıştır.
