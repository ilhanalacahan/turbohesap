# ThCheckbox

> Erişilebilir, klavye uyumlu ve FontAwesome ikon desteğiyle tasarlanmış özelleştirilmiş seçim kutusu (checkbox) bileşeni.

- **Sınıf**: `th-checkbox` (+ `--sm|lg`), `th-checkbox-wrap`, `th-checkbox-label`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThCheckbox.razor`, `ThCheckbox.razor.cs`, `Frontend/css/input.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Label` | `string?` | — | Seçim kutusunun yanındaki metin etiketi |
| `ChildContent` | `RenderFragment?` | — | Alternatif etiket içeriği |
| `Value` | `bool` | `false` | Seçim değeri (`@bind-Value` desteklenir) |
| `ValueChanged` | `EventCallback<bool>` | — | Seçim değiştiğinde tetiklenen olay |
| `Disabled` | `bool` | `false` | Etkisizleştirilme durumu |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Giriş kutusuna ve etikete aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThCheckbox @bind-Value="_isAccepted" Label="Kullanıcı sözleşmesini kabul ediyorum" />
```

## Kullanılan token'lar
- `--th-border-strong`
- `--th-primary`
- `--th-primary-fg`
- `--th-radius-sm`
- `--th-surface`
- `--th-surface-3`
- `--th-ring`
- `--th-font-sans`
- `--th-text`

## Tema/uyum notları
- **Erişilebilirlik**: Standart HTML `<input type="checkbox">` etiketini temel alır; ekran okuyucular ve form doğrulayıcılar tarafından doğrudan tanınır ve klavye gezinmesini destekler.
- **Koyu Tema**: Checkbox çerçeveleri, zemin rengi ve aktif tık rengi semantik token'lara bağlı olup tema tasarımcısıyla uyumludur.
