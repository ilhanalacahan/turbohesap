# ThCard

> Başlık, alt başlık, başlık aksiyon yuvası, gövde ve sağa hizalı alt eylem (footer) yuvasıyla standartlaştırılmış kart / panel bileşeni.

- **Sınıf**: `th-card`, `th-panel-header`, `th-panel-title`, `th-panel-subtitle`, `th-card__body`, `th-card__footer`
- **Namespace**: `Turbohesap.Web.Components.Card`
- **Dosyalar**: `ThCard.razor`, `ThCard.razor.cs`, `Frontend/css/card.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Title` | `string?` | — | Kart başlığı (Header verilmediğinde kullanılır) |
| `Subtitle` | `string?` | — | Kart alt başlığı (Header verilmediğinde kullanılır) |
| `Icon` | `string?` | — | Başlık metninin solunda gösterilecek FontAwesome ikonu sınıfı |
| `Header` | `RenderFragment?` | — | Özel başlık alanı yuvası (Title/Subtitle yerine geçer) |
| `HeaderActions` | `RenderFragment?` | — | Başlığın sağ tarafına eklenecek aksiyon butonları yuvası |
| `ChildContent` | `RenderFragment?` | — | Kart gövdesi içeriği |
| `Footer` | `RenderFragment?` | — | Kart alt eylem alanı yuvası |
| `NoPadding` | `bool` | `false` | Kart gövdesi iç boşluğunu (padding) kaldırarak içeriğin sıfıra sıfır sarmalanmasını sağlar |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThCard Title="Müşteri Detayı" Subtitle="Müşteri kartı temel bilgileri">
    <HeaderActions>
        <ThButton Variant="ButtonVariant.Ghost" Size="ComponentSize.Sm" Icon="fa-solid fa-pen">Düzenle</ThButton>
    </HeaderActions>
    <ChildContent>
        <p>Müşteri adı: Cihad Özhan</p>
    </ChildContent>
    <Footer>
        <ThButton Variant="ButtonVariant.Secondary">İptal</ThButton>
        <ThButton Variant="ButtonVariant.Primary">Kaydet</ThButton>
    </Footer>
</ThCard>
```

## Kullanılan token'lar
- `--th-surface`
- `--th-surface-2`
- `--th-border`
- `--th-radius-lg`
- `--th-card-shadow`
- `--th-text`
- `--th-text-muted`
- `--th-fw-semibold`

## Tema/uyum notları
- **Gölge ve Köşeler**: Tema tasarımcısında ayarlanan gölge düzeyi (`--th-card-shadow`) ve köşe yuvarlaklığı (`--th-radius-lg`) ile tam uyumlu çalışır.
- **İkincil Panel (Footer)**: Footer alanı, alt kenarlıkla ayrılır ve `--th-surface-2` ikincil renk zeminini alır.
