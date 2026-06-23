# ThCurrencyInput

> Binlik ayraç desteği ve entegre hesap makinesi diyaloguna sahip estetik para birimi giriş bileşeni.

- **Sınıf**: `th-currency-input-wrapper`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThCurrencyInput.razor`, `ThCurrencyInput.razor.cs`, `Frontend/css/currencyinput.css`, JS Interop (`initCurrencyInput`, `setCurrencyInputValue`, `disposeCurrencyInput`)

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `decimal?` | — | Giriş kutusuna bağlı iki yönlü değer. |
| `ValueChanged` | `EventCallback<decimal?>` | — | Değer değiştiğinde tetiklenen olay. |
| `Label` | `string` | `""` | Giriş kutusunun üstündeki başlık metni. |
| `Placeholder` | `string` | `"0,00"` | Boş durumdaki yer tutucu metin. |
| `CurrencySymbol` | `string` | `"₺"` | Sol tarafta gösterilecek para birimi sembolü. |
| `ShowCalculator` | `bool` | `true` | Sağ tarafta hesap makinesi açma butonunun gösterilip gösterilmeyeceği. |
| `Disabled` | `bool` | `false` | Giriş kutusunun devre dışı olma durumu. |
| `Size` | `ComponentSize` | `ComponentSize.Md` | Bileşen boyutu. |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase). |

## Gelişmiş Davranışlar ve Senkronizasyon
*   **Yazarken Anlık Formatlama (Live Format)**: Kullanıcı sayı tuşladıkça değer anlık olarak binlik ayraçlı kuruş hassasiyetli Türkçe finans formatına (`12.500,50`) dönüştürülür.
*   **İmleç ve DOM Koruma (Cursor Position)**: Blazor'ın DOM input değerlerini ezerek yazım akışını bozmasını önlemek amacıyla değer güncellemeleri asenkron JS interop (`setCurrencyInputValue`) üzerinden senkron tutulur.
*   **Kompakt Hesap Makinesi**: Entegre hesap makinesi, hantal modal kutuları yerine ekranı hafifçe karartan yarı saydam bir overlay ve son derece kompakt, başlık alanı daraltılmış özel bir diyalog penceresiyle açılır.

## Kullanım
```razor
<ThCurrencyInput Value="_faturaTutari" 
                 ValueChanged="val => _faturaTutari = val"
                 Label="Fatura Tutarı" 
                 CurrencySymbol="₺" 
                 ShowCalculator="true" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-border`, `--th-radius-md`, `--th-radius-lg`, `--th-shadow-xl`, `--th-text-subtle`, `--th-text-muted`, `--th-primary`, `--th-primary-subtle`, `--th-font-mono`.

## Tema/uyum notları
Yazma anındaki pürüzsüz interop senkronizasyonuna dayanır. Koyu tema ve farklı ekran genişlikleriyle tam uyumludur.
