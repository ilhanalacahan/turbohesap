# ThTrending

> Finansal veriler, satış ciro karşılaştırmaları ve büyüme oranları için yönlü ikon barındıran trend değişim göstergesi rozeti.

- **Sınıf**: `th-trending` (+ `--sm|md|lg`, `--up|--down|--flat`)
- **Namespace**: `Turbohesap.Web.Components.Badge`
- **Dosyalar**: `ThTrending.razor`, `ThTrending.razor.cs`, `Frontend/css/trending.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `double` | `0` | Yüzdesel değişim/trend değeri (Ör: `12.5`, `-3.4`). |
| `ShowPercentSign` | `bool` | `true` | Önüne `%` işaretinin konulma durumu. |
| `Format` | `string` | `"F1"` | Ondalık hanesini biçimlendirecek C# format dizgesi. |
| `Size` | `ComponentSize` | `Sm` | Boyut varyantı (`Sm`, `Md`, `Lg`). |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Yön Durum Eşlemeleri
*   **Up (Yeşil)**: Değer `> 0` ise yeşil arka plan ve yukarı yönlü trend oku (`fa-arrow-trend-up`).
*   **Down (Kırmızı)**: Değer `< 0` ise kırmızı arka plan ve aşağı yönlü trend oku (`fa-arrow-trend-down`).
*   **Flat (Gri)**: Değer `== 0` ise nötr arka plan ve düz tire çizgisi (`fa-minus`).

## Kullanım
```razor
<ThTrending Value="14.8" Size="ComponentSize.Md" />
<ThTrending Value="-2.5" Format="F2" />
```

## Kullanılan token'lar
`--th-success`, `--th-success-subtle`, `--th-danger`, `--th-danger-subtle`, `--th-surface-2`, `--th-border`, `--th-text-muted`, `--th-radius-full`, `--th-fw-semibold`.

## Tema/uyum notları
Renk/yarıçap/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık ve koyu temalarda test edilmiş ve tam uyumluluk sağlanmıştır.
