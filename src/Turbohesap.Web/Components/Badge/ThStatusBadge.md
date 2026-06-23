# ThStatusBadge

> ERP ve muhasebe durum verilerini (Taslak, Onay Bekliyor, Ödendi vb.) otomatik eşleyerek standartlaştıran ve renklendiren akıllı rozet bileşeni.

- **Sınıf**: `th-status-badge` (+ `--sm|md|lg`, `--success|warning|danger|info|accent|neutral`)
- **Namespace**: `Turbohesap.Web.Components.Badge`
- **Dosyalar**: `ThStatusBadge.razor`, `ThStatusBadge.razor.cs`, `Frontend/css/statusbadge.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Status` | `string` | — | Durum anahtar kodu (Ör: `"TASLAK"`, `"PENDING"`, `"ODENDI"`). Case-insensitive çalışır. |
| `Label` | `string` | — | Ekranda gösterilecek özel metin. Boş bırakılırsa durum kodu otomatik formatlanır (örn: `"ONAY_BEKLIYOR"` -> `"Onay Bekliyor"`). |
| `ShowDot` | `bool` | `true` | Sol taraftaki durum gösterge noktasının (dot) görünürlüğü. |
| `Size` | `ComponentSize` | `Md` | Boyut varyantı (`Sm`, `Md`, `Lg`). |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Durum Kodu Eşlemeleri
*   **Neutral (Gri)**: `DRAFT`, `TASLAK`
*   **Warning (Sarı)**: `PENDING`, `ONAY_BEKLIYOR`, `BEKLEMEDE`, `WAITING`
*   **Success (Yeşil)**: `COMPLETED`, `ONAYLANDI`, `ODENDI`, `TAMAMLANDI`, `ACTIVE`, `AKTIF`
*   **Danger (Kırmızı)**: `CANCELLED`, `IPTAL`, `REDDEDILDI`, `REJECTED`, `HATA`
*   **Info (Mavi)**: `PROCESSING`, `INVOICED`, `SEVK_EDILDI`, `DEVAM_EDIYOR`
*   **Accent (Mor)**: `PARTIAL`, `KISMEN_ODENDI`, `KISMEN_SEVK_EDILDI`

## Kullanım
```razor
<ThStatusBadge Status="ONAY_BEKLIYOR" />
<ThStatusBadge Status="ODENDI" Label="Fatura Ödendi" ShowDot="false" />
```

## Kullanılan token'lar
`--th-success`, `--th-success-subtle`, `--th-warning`, `--th-warning-subtle`, `--th-danger`, `--th-danger-subtle`, `--th-info`, `--th-info-subtle`, `--th-primary`, `--th-primary-subtle`, `--th-surface-2`, `--th-border`, `--th-text-muted`, `--th-text-subtle`, `--th-radius-full`, `--th-density-gap`, `--th-fw-semibold`.

## Tema/uyum notları
Renk/yarıçap/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık ve koyu temalarda test edilmiş ve tam uyumluluk sağlanmıştır.
