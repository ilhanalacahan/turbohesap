# ThAlert

> Bilgilendirme amaçlı inline uyarı, başarı, tehlike ve ikaz kutuları sarmalayıcısı.

- **Sınıf**: `th-alert` (+ `--info|success|warning|danger`), `th-alert__icon`, `th-alert__content`, `th-alert__title`, `th-alert__description`, `th-alert__close`
- **Namespace**: `Turbohesap.Web.Components.Alert`
- **Dosyalar**: `ThAlert.razor`, `ThAlert.razor.cs`, `Frontend/css/alert.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Title` | `string?` | — | Uyarı panelinin kalın başlığı |
| `ChildContent` | `RenderFragment?` | — | Uyarı açıklaması içeriği yuvası |
| `Variant` | `AlertVariant` | `Info` | Görsel varyasyon rengi (`Info`, `Success`, `Warning`, `Danger`) |
| `ShowIcon` | `bool` | `true` | Varyant tipine göre otomatik uygun durum ikonunu (FontAwesome) gösterir |
| `Closable` | `bool` | `false` | Sağ üst köşeye kapatma butonu yerleştirir |
| `OnClose` | `EventCallback` | — | Kapatma butonu tıklandığında tetiklenen olay |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThAlert Variant="AlertVariant.Success" Title="Başarılı!" Closable="true">
    Veriler başarıyla kaydedildi ve veritabanı güncellendi.
</ThAlert>
```

## Kullanılan token'lar
- `--th-info` / `--th-info-subtle` / `--th-info-fg`
- `--th-success` / `--th-success-subtle` / `--th-success-fg`
- `--th-warning` / `--th-warning-subtle` / `--th-warning-fg`
- `--th-danger` / `--th-danger-subtle` / `--th-danger-fg`
- `--th-border`
- `--th-radius-md`
- `--th-text`
- `--th-text-muted`
- `--th-transition`
- `--th-ease`
- `--th-font-sans`

## Tema/uyum notları
- **Dinamik İkonlar**: `Variant` parametresine bağlı olarak (Information, Check, Warning, Cross) ikonları otomatik bind edilir.
- **Koyu Tema**: Arka plan ve yazı renkleri semantik durum değişkenlerine (`-subtle` ve `-fg`) bağlı olup karanlık modda okunabilirliği korur.
