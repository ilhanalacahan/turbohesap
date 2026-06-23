# ThAddressPicker

> Depo yönetiminde raf adreslerini (Bölge-Koridor-Raf-Kat) görsel ve etkileşimli olarak seçtiren gelişmiş giriş bileşeni.

- **Sınıf**: `th-address-picker`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThAddressPicker.razor`, `ThAddressPicker.razor.cs`, `Frontend/css/addresspicker.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `string` | — | Seçilen adres verisi (Format: Bölge-Koridor-Raf-Kat, Ör: `A-03-B-04`) |
| `ValueChanged` | `EventCallback<string>` | — | Adres değiştiğinde tetiklenen olay |
| `ValueExpression` | `Expression<Func<string>>` | — | Blazor form binding için ifade |
| `Placeholder` | `string` | `"Depo Adresi Seçin"` | Adres seçilmediğinde gösterilecek metin |
| `Disabled` | `bool` | `false` | Girişin pasif olma durumu |
| `Areas` | `List<string>` | `["A", "B", "C", "D"]` | Mevcut Bölgeler (Bölümler) |
| `MaxRows` | `int` | `9` | Maksimum Koridor sayısı |
| `Shelves` | `List<string>` | `["A", "B", "C", "D", "E", "F"]` | Yatay Raflar |
| `MaxLevels` | `int` | `5` | Dikey Katlar |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |

## Kullanım
```razor
<ThAddressPicker @bind-Value="myAddress" 
                 Areas="@(new() { "A", "B", "C" })"
                 MaxRows="5"
                 MaxLevels="4" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-surface-3`, `--th-border`, `--th-border-strong`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-primary`, `--th-primary-hover`, `--th-primary-fg`, `--th-ring`, `--th-radius-xs`, `--th-radius-sm`, `--th-radius-md`, `--th-radius-lg`, `--th-radius-full`, `--th-shadow-sm`, `--th-shadow-lg`, `--th-control-height`, `--th-density-gap`, `--th-content-padding`, `--th-transition-fast`, `--th-ease`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık ve koyu temalarda test edilmiş ve tam uyumluluk sağlanmıştır.
