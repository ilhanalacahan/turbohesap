# ThShortcutHint

> Klavye kısayollarını (Ctrl+S, ESC, vb.) görsel olarak ayrıştırılmış kbd kutularıyla şık bir şekilde sunan ipucu göstergesi.

- **Sınıf**: `th-shortcut-hint` (+ `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Badge`
- **Dosyalar**: `ThShortcutHint.razor`, `ThShortcutHint.razor.cs`, `Frontend/css/shortcuthint.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Keys` | `string` | — | Kısayol tuş kombinasyonu metni (Örn: `"Ctrl+S"`, `"Alt + F4"`, `"Enter"`). Artı (`+`) veya eksi (`-`) işaretlerine göre otomatik olarak ayrı tuşlara bölünür. |
| `Size` | `ComponentSize` | `Sm` | Tuşların boyut varyantı (`Sm`, `Md`, `Lg`). |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Kullanım
```razor
<ThShortcutHint Keys="Ctrl+Enter" />
<ThShortcutHint Keys="Esc" Size="ComponentSize.Md" />
```

## Kullanılan token'lar
`--th-surface-2`, `--th-border-strong`, `--th-text-muted`, `--th-text-subtle`, `--th-radius-xs`, `--th-shadow-xs`, `--th-font-sans`, `--th-fw-semibold`, `--th-fw-medium`.

## Tema/uyum notları
Renk/yarıçap/boyut token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Tuşların alt kenarında gölgeli 3D efekt verilerek gerçekçi klavye tuşu (kbd) görünümü elde edilmiştir. Açık ve koyu temalarda test edilmiştir.
