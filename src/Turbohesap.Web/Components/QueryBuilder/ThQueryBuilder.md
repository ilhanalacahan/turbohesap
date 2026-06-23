# ThQueryBuilder

> AI agents ve web servisleri tarafından serileştirilerek kullanılabilecek ortak QueryGroup/QueryRule modelleriyle çalışan, özyinelemeli (AND/OR) mantıksal gruplama ve operatör kuralları içeren query builder bileşeni.

- **Sınıf**: `th-query-builder`, `th-query-group`, `th-query-rule`, `th-query-empty`
- **Namespace**: `Turbohesap.Web.Components.QueryBuilder`
- **Dosyalar**: `ThQueryBuilder.razor`, `ThQueryBuilder.razor.cs`, `Frontend/css/query-builder.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Fields` | `List<QueryBuilderField>` | `[]` | Filtrelenebilir alanların listesi (ad, etiket, tip) |
| `Value` | `QueryGroup` | `new()` | Filtreleme kural grubu veri yapısı (`@bind-Value` desteklenir) |
| `ValueChanged` | `EventCallback<QueryGroup>` | — | Filtre yapısı değiştiğinde tetiklenen olay |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri (TurboComponentBase) |

## Kullanım

```razor
<ThQueryBuilder Fields="_queryBuilderFields" Value="_queryGroup" ValueChanged="OnQueryGroupChanged" />
```

Ortak veri modelleri ve `IQueryable` uzantıları `Turbohesap.Shared` katmanında yer alır:
- `QueryGroup`: AND/OR operatörü altında toplanan kuralları (`QueryRule`) ve alt grupları (`QueryGroup`) barındırır.
- `QueryRule`: Filtre alanı, operatörü, veri tipi ve karşılaştırma değerini barındırır.

`QueryBuilderExpressionBuilder` motoru yardımıyla bu kurallar:
- Dinamik LINQ Expression ağaçlarına (`Expression<Func<T, bool>>`)
- SQL WHERE cümlelerine
- LINQ önizleme dizelerine
çevrilebilir.

## Kullanılan token'lar
- `--th-border`
- `--th-border-strong`
- `--th-radius-lg`
- `--th-radius-md`
- `--th-radius-sm`
- `--th-surface`
- `--th-surface-2`
- `--th-surface-3`
- `--th-text`
- `--th-text-muted`
- `--th-text-subtle`
- `--th-primary`
- `--th-primary-fg`
- `--th-shadow-sm`
- `--th-transition-fast`
- `--th-ease`
- `--th-fw-semibold`
- `--th-font-sans`

## Tema/uyum notları
- **Tipografi**: Tüm filtre elemanları, operatör butonları, seçim listeleri ve giriş kutuları sistemin `--th-font-sans` tokenını ve form giriş standardını (`th-input--sm`) takip eder. Tema yazı tipi değiştikçe anında güncellenir.
- **Görsel Derinlik**: İç içe eklenen her alt grup, border ve indent (`margin-inline-start`) marjları yanında zemin rengini değiştirerek (`--th-surface-2` / `--th-surface-3`) katmanlı bir görünüm oluşturur.
- **Koyu Tema**: Sabit renk kodları barındırmaz, tüm renkler ve kenarlıklar aktif temayla tam uyumludur.
