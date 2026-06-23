# ThAmountDisplay

> Para birimleri ve sayısal miktarları, kuruş (ondalık) kısmını küçülterek ve işaretine göre (borç/alacak) renklendirerek gösteren finansal tutar bileşeni.

- **Sınıf**: `th-amount-display` (+ `--sm|md|lg`, `--positive|--negative|--neutral`)
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThAmountDisplay.razor`, `ThAmountDisplay.razor.cs`, `Frontend/css/amountdisplay.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Amount` | `decimal` | `0` | Gösterilecek sayısal/finansal değer. |
| `CurrencySymbol` | `string` | `"₺"` | Para birimi simgesi. |
| `SymbolPosition` | `SymbolPosition` | `SymbolPosition.Append` | Simgenin sayının önünde mi (`Prepend`) yoksa sonunda mı (`Append`) duracağı. |
| `ColorCode` | `bool` | `true` | Pozitif miktarları yeşil, negatif miktarları kırmızı renklendirme durumu. |
| `Culture` | `CultureInfo` | `tr-TR` | Sayı formatlama için kullanılacak kültürel ayarlar. |
| `Size` | `ComponentSize` | `Md` | Yazı boyutu varyantı (`Sm`, `Md`, `Lg`). |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Kullanım
```razor
<ThAmountDisplay Amount="12500.75M" 
                 CurrencySymbol="₺" 
                 ColorCode="true" 
                 Size="ComponentSize.Lg" />
```

## Kullanılan token'lar
`--th-success`, `--th-danger`, `--th-text`, `--th-font-mono`, `--th-fw-semibold`, `--th-fw-medium`, `--th-transition-fast`, `--th-ease`.

## Tema/uyum notları
Renk/yarıçap/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Rakamların her satırda tam hizalı (tabular-nums) durması için varsayılan olarak monospaced font token'ını kullanır.
