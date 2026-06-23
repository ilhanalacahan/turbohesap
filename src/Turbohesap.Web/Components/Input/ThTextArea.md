# ThTextArea

> Çok satırlı metin giriş kutusu (textarea) bileşeni.

- **Sınıf**: `th-textarea` (+ `--sm|lg`)
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThTextArea.razor`, `ThTextArea.razor.cs`, `Frontend/css/input.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Placeholder` | `string?` | — | Giriş alanındaki arka plan ipucu metni |
| `Rows` | `int` | `3` | Görünür satır yüksekliği |
| `Value` | `string` | — | Girilen metin değeri (`@bind-Value` desteklenir) |
| `ValueChanged` | `EventCallback<string>` | — | Değer değiştiğinde tetiklenen olay |
| `Disabled` | `bool` | `false` | Etkisizleştirilme durumu |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Element köküne aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThTextArea @bind-Value="_notes" Placeholder="Notlarınızı buraya yazınız..." Rows="4" />
```

## Kullanılan token'lar
- `--th-border-strong`
- `--th-primary`
- `--th-ring`
- `--th-surface`
- `--th-surface-3`
- `--th-text`
- `--th-text-subtle`
- `--th-radius-md`
- `--th-transition-fast`
- `--th-ease`

## Tema/uyum notları
- **Erişilebilirlik**: Standart HTML `<textarea>` etiketini temel alır.
- **Koyu Tema & Odaklanma**: Aktif tema ayarlarındaki kenarlık renklerine, oda halkası rengine (`--th-ring`) ve arka plan renklerine otomatik uyum sağlar.
