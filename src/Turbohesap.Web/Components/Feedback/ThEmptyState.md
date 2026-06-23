# ThEmptyState

> Arama sonuçlarında eşleşen veri çıkmadığında veya yeni açılan modüllerde henüz kayıt olmadığında kullanıcıyı bilgilendiren boş veri paneli şablonu.

- **Sınıf**: `th-empty-state` (+ `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Feedback`
- **Dosyalar**: `ThEmptyState.razor`, `ThEmptyState.razor.cs`, `Frontend/css/emptystate.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Icon` | `string` | `"fa-regular fa-folder-open"` | Gösterilecek FontAwesome ikon sınıf adı. |
| `Title` | `string` | `"Veri Bulunamadı"` | Panel ana başlığı. |
| `Description` | `string` | — | Başlığın altında gösterilecek detaylı yönlendirme metni. |
| `ChildContent` | `RenderFragment` | — | Alt kısımda eylemleri (örn: "Ekle" butonunu) barındıracak alan. |
| `Size` | `ComponentSize` | `Md` | Panel dolgu ve boyut varyantı (`Sm`, `Md`, `Lg`). |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## Kullanım
```razor
<ThEmptyState Icon="fa-solid fa-users-slash" 
              Title="Kayıtlı Müşteri Yok" 
              Description="Henüz cari hesap kartı eklememişsiniz. İlk müşterinizi hemen ekleyin."
              Size="ComponentSize.Md">
    <ThButton Variant="ButtonVariant.Primary" OnClick="OpenAddModal">Yeni Müşteri Ekle</ThButton>
</ThEmptyState>
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-border-strong`, `--th-text`, `--th-text-subtle`, `--th-text-muted`, `--th-radius-lg`, `--th-radius-full`, `--th-transition`, `--th-ease`, `--th-fw-bold`, `--th-content-padding`, `--th-density-gap`.

## Tema/uyum notları
Renk/yarıçap/yoğunluk/gölge token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Panel ikon alanı hover yapıldığında hafif ölçeklenme (`transform: scale(1.05)`) animasyonu sunar. Açık ve koyu temalarda test edilmiştir.
