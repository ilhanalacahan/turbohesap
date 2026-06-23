# ThSwitch

> Sürüklemeden, tek tıklamayla veya klavye boşluk tuşuyla tetiklenen, erişilebilir ve özelleştirilmiş anahtar (switch) bileşeni.

- **Sınıf**: `th-switch` (+ `--sm|lg`), `th-switch-container`, `th-switch-label`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThSwitch.razor`, `ThSwitch.razor.cs`, `Frontend/css/input.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Label` | `string?` | — | Anahtarın yanındaki metin etiketi |
| `ChildContent` | `RenderFragment?` | — | Alternatif etiket içeriği |
| `Value` | `bool` | `false` | Seçim değeri (`@bind-Value` desteklenir) |
| `ValueChanged` | `EventCallback<bool>` | — | Seçim değiştiğinde tetiklenen olay |
| `Disabled` | `bool` | `false` | Etkisizleştirilme durumu |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Butona/Etikete aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThSwitch @bind-Value="_isActive" Label="Durum aktif mi" Size="ComponentSize.Sm" />
```

## Kullanılan token'lar
- `--th-border-strong`
- `--th-primary`
- `--th-radius-full`
- `--th-shadow-sm`
- `--th-transition`
- `--th-ease`
- `--th-font-sans`
- `--th-text`

## Tema/uyum notları
- **Erişilebilirlik**: `button` etiketi kullanılarak oluşturulmuştur; `role="switch"` ve `aria-checked` nitelikleriyle ekran okuyucu ve klavye etkileşimi (Space/Enter) ile tam uyumludur.
- **Koyu Tema**: Arka plan ve aktifleşme renkleri semantik renklere bağlıdır; tema geçişlerinde otomatik güncellenir.
