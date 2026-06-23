# ThFunnelChart

> Satış fırsatları, teklif süreçleri ve operasyon adımlarının dönüşüm oranlarını yamuk katmanlarla görselleştiren dikey huni grafiği.

- **Sınıf**: `th-funnel-chart`
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThFunnelChart.razor`, `ThFunnelChart.razor.cs`, `Frontend/css/funnelchart.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Title` | `string` | `"Dönüşüm Hunisi"` | Grafik başlığı |
| `Stages` | `List<FunnelStage>` | `new()` | Huniyi oluşturacak aşama verileri listesi |

### FunnelStage Modeli
- `Label` (`string`): Aşama adı (Örn: "Ziyaretçi", "Teklif").
- `Value` (`double`): Aşamaya ait sayısal değer (Huni taban genişliği buna oranlanır).
- `Description` (`string`): Sağ tarafta gösterilecek ufak açıklama metni.

## Kullanım
```razor
<ThFunnelChart Title="Satış Fırsat Hunisi" 
               Stages="@_salesStages" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-border`, `--th-border-strong`, `--th-radius-lg`, `--th-card-shadow`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-primary`, `--th-success`, `--th-transition-fast`.

## Tema/uyum notları
Dönüşüm aşamalarını birincil renkten (`var(--th-primary)`) başlayarak başarı rengine (`var(--th-success)`) doğru harmanlanan katmanlarla (`color-mix` aracılığıyla) otomatik boyar. Koyu ve açık modlarla tam uyumludur.
