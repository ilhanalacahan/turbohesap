# ThKpiCard

> Entegre mikro çizgi grafikli (Sparkline) akıllı metrik ve performans kartı.

- **Sınıf**: `th-kpi-card` (+ `--trend-up`, `--trend-down`)
- **Namespace**: `Turbohesap.Web.Components.Card`
- **Dosyalar**: `ThKpiCard.razor`, `ThKpiCard.razor.cs`, `Frontend/css/kpicard.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Title` | `string` | — | Kartın üst başlığı (Örn: "Toplam Kasa") |
| `Value` | `string` | — | Gösterilecek büyük değer (Örn: "125.000 ₺") |
| `Subtitle` | `string` | — | Alt açıklama yazısı |
| `TrendValue` | `decimal?` | — | Yüzdesel değişim trend oranı (Örn: 0.15 = %15 artış, -0.05 = %5 azalış) |
| `TrendLabel` | `string` | — | Trend değerinin yanındaki ek açıklama (Örn: "geçen aya göre") |
| `Icon` | `string` | — | Sağ üstte gösterilecek opsiyonel FontAwesome ikon sınıfı |
| `SparklineData` | `List<decimal>?` | — | Trend çizgisini oluşturacak sayı listesi. Minimum 2 nokta olmalıdır. |

## Kullanım
```razor
<ThKpiCard Title="Haftalık Satış" 
           Value="45.230,00 ₺" 
           TrendValue="0.124" 
           TrendLabel="geçen haftaya göre"
           Icon="fa-solid fa-chart-line"
           SparklineData="@(new() { 12000, 15000, 14000, 19000, 25000, 32000, 45230 })" />
```

## Kullanılan token'lar
`--th-surface`, `--th-border-strong`, `--th-radius-lg`, `--th-card-shadow`, `--th-text`, `--th-text-muted`, `--th-success`, `--th-danger`, `--th-success-subtle`, `--th-danger-subtle`.

## Tema/uyum notları
Açık ve koyu tema renk token'larına doğrudan bağlıdır. Trend değerine göre SVG çizgi rengini otomatik olarak yeşile (`var(--th-success)`) veya kırmızıya (`var(--th-danger)`) boyar.
