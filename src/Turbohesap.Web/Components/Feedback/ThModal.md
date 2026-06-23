# ThModal

> Gelişmiş, taşınabilir (draggable), tam ekran yapılabilir, soft renkli ve dinamik form davranışına (C# DialogResult) sahip modal sarmalayıcı bileşeni.

- **Sınıf**: `th-modal` (+ varyantlar, boyutlar)
- **Namespace**: `Turbohesap.Web.Components.Feedback`
- **Dosyalar**: `ThModal.razor`, `ThModal.razor.cs`, `Frontend/css/modal.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Visible` | `bool` | `false` | Modalın görünürlüğü. |
| `VisibleChanged` | `EventCallback<bool>` | - | Görünürlük değiştiğinde tetiklenen olay. |
| `Title` | `string?` | - | Modal başlığı. |
| `Subtitle` | `string?` | - | Modal alt başlığı (opsiyonel). |
| `Size` | `ModalSize` | `ModalSize.Md` | Modal genişliği (`Sm`, `Md`, `Lg`, `Xl`). |
| `Draggable` | `bool` | `false` | Modalın başlığından tutularak sürüklenebilmesi (taşınabilirlik). |
| `Fullscreen` | `bool` | `false` | Modalın doğrudan tam ekran açılması. |
| `Maximizable` | `bool` | `false` | Başlıkta ekranı kaplama/küçültme butonunun gösterilmesi. |
| `CloseOnOverlay` | `bool` | `true` | Arka plana (overlay) tıklandığında modalın kapanması. |
| `Variant` | `ModalVariant` | `ModalVariant.Default` | Başlığın soft renklendirilmesi (`Default`, `Primary`, `Secondary`, `Success`, `Warning`, `Danger`, `Info`). |
| `ChildContent` | `RenderFragment?` | - | Modal gövde içeriği. |
| `Footer` | `RenderFragment?` | - | Modal alt bilgi şeridi içeriği. |

## Varyantlar / Boyutlar
- Boyut Sınıfları: `.th-modal--sm`, `.th-modal--md`, `.th-modal--lg`, `.th-modal--xl`.
- Varyant Sınıfları (Soft Renkli Başlıklar): `.th-modal--variant-primary`, `.th-modal--variant-success`, `.th-modal--variant-danger`, `.th-modal--variant-warning`, `.th-modal--variant-info`.

## Kullanım

### 1. Statik/Satır İçi Kullanım (Inline Markup)
```razor
<ThModal @bind-Visible="_modalVisible" Title="Kullanıcı Bilgileri" Size="ModalSize.Md" Maximizable="true">
    <ChildContent>
        <p>İçerik buraya gelecektir...</p>
    </ChildContent>
    <Footer>
        <ThButton Variant="ButtonVariant.Ghost" OnClick="@(() => _modalVisible = false)">Kapat</ThButton>
    </Footer>
</ThModal>
```

### 2. Dinamik Form Kullanımı (C# Form gibi sonuç değerleri ile)
```csharp
@inject ThModalService ModalService

private async Task OpenFormAsync()
{
    var options = new ModalOptions { Size = ModalSize.Md, Draggable = true, Maximizable = true };
    var parameters = new Dictionary<string, object> { ["Id"] = 42 };
    
    var modalRef = ModalService.Show<CustomerEditForm>("Müşteri Bilgileri", parameters, options);
    var result = await modalRef.Result;
    
    if (!result.Cancelled)
    {
        var data = result.Data as CustomerDto;
        // Sonuç verisiyle işlem yap (DialogResult gibi)...
    }
}
```

*Not: İçerik bileşeninin modalı kapatıp veri döndürebilmesi için cascaded referansı alması gerekir:*
```csharp
[CascadingParameter] public ThModalRef? ModalRef { get; set; }

private void Save()
{
    ModalRef?.Close(ModalResult.Ok(new { Saved = true }));
}
```

## Kullanılan token'lar
- `--th-overlay`
- `--th-surface`
- `--th-surface-2`
- `--th-surface-3`
- `--th-border`
- `--th-radius-xl`
- `--th-shadow-xl`
- `--th-text`
- `--th-text-subtle`
- `--th-primary`
- `--th-primary-subtle`
- `--th-success`
- `--th-success-subtle`
- `--th-danger`
- `--th-danger-subtle`
- `--th-warning`
- `--th-warning-subtle`
- `--th-info`
- `--th-info-subtle`
- `--th-transition`
- `--th-ease`

## Tema/uyum notları
- Koyu temada arka plan, kenarlıklar ve soft renkli başlıklar otomatik olarak koyu tema tonlarına (`rgba(...)` / `color-mix` aracılığıyla) uyum sağlar.
- Yoğunluk (density) ve global ölçek (scale) ayarları modal boyutlarını ve padding mesafelerini otomatik olarak yeniden hesaplar.
