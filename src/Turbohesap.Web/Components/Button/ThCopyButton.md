# ThCopyButton

> Referans kodları, vergi numaraları ve IBAN'lar gibi metinsel verileri tek tıkla kopyalayan ve onay animasyonu veren kopyalama butonu.

- **Sınıf**: `th-copy-button` (+ `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Button`
- **Dosyalar**: `ThCopyButton.razor`, `ThCopyButton.razor.cs`, `Frontend/css/copybutton.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Text` | `string` | — | Kopyalanacak metin değeri |
| `Size` | `ComponentSize` | `Sm` | Buton boyutu (`Sm`, `Md`, `Lg`) |
| `TooltipText` | `string` | `"Kopyala"` | Kopyalamadan önce gösterilecek ipucu metni |
| `SuccessTooltipText` | `string` | `"Kopyalandı!"` | Kopyalama başarılı olunca gösterilecek ipucu metni |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Kullanım
```razor
<ThCopyButton Text="TR26000100000000000000000000" TooltipText="IBAN Kopyala" />
```

## Kullanılan token'lar
`--th-surface-3`, `--th-border`, `--th-text-muted`, `--th-primary`, `--th-success`, `--th-success-subtle`, `--th-radius-sm`, `--th-transition-fast`, `--th-ease`.

## Tema/uyum notları
Renk/yarıçap/gölge token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Tıklama anında ebeveyn tıklama olayını (`click bubble`) engellediği için veri tabloları ve tıklanabilir satır listelerinde güvenle kullanılır.
