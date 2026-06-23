# ThDrawer

> Pozisyonlanabilir (sağ, sol, üst, alt), genişletilebilir, soft renkli ve dinamik form davranışına (C# DrawerResult) sahip gelişmiş yan panel (drawer) bileşeni.

- **Sınıf**: `th-drawer` (+ varyantlar, boyutlar)
- **Namespace**: `Turbohesap.Web.Components.Feedback`
- **Dosyalar**: `ThDrawer.razor`, `ThDrawer.razor.cs`, `Frontend/css/drawer.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Visible` | `bool` | `false` | Yan panelin görünürlüğü. |
| `VisibleChanged` | `EventCallback<bool>` | — | Görünürlük değiştiğinde tetiklenen olay. |
| `Title` | `string?` | — | Yan panel başlığı. |
| `Subtitle` | `string?` | — | Yan panel alt başlığı (opsiyonel). |
| `Position` | `DrawerPosition` | `DrawerPosition.Right` | Panel açılış yönü (`Right`, `Left`, `Top`, `Bottom`). |
| `Size` | `DrawerSize` | `DrawerSize.Md` | Panel boyutu (`Sm`, `Md`, `Lg`, `Xl`, `Fullscreen`). |
| `Expandable` | `bool` | `false` | Sürüklenebilir kenar handle'ı ile serbestçe genişletilebilir özellik. |
| `CloseOnOverlay` | `bool` | `true` | Arka plana tıklandığında yan panelin kapanması. |
| `Variant` | `ModalVariant` | `ModalVariant.Default` | Başlığın soft renklendirilmesi (`Default`, `Primary`, `Secondary`, `Success`, `Warning`, `Danger`, `Info`). |
| `ChildContent` | `RenderFragment?` | — | Yan panel gövde içeriği. |
| `Footer` | `RenderFragment?` | — | Yan panel alt bilgi şeridi içeriği. |

## Varyantlar / Boyutlar
- Yön Sınıfları: `.th-drawer--right`, `.th-drawer--left`, `.th-drawer--top`, `.th-drawer--bottom`.
- Boyut Sınıfları (Genişlik/Yükseklik): `.th-drawer--sm`, `.th-drawer--md`, `.th-drawer--lg`, `.th-drawer--xl`, `.th-drawer--fullscreen`, `.th-drawer--expanded`.
- Başlık Varyantları: `.th-modal--variant-primary`, `.th-modal--variant-success`, `.th-modal--variant-danger`, `.th-modal--variant-warning`, `.th-modal--variant-info`.

## Kullanım

### 1. Statik/Satır İçi Kullanım (Inline Markup)
```razor
<ThDrawer @bind-Visible="_drawerVisible" Title="Filtre Seçenekleri" Position="DrawerPosition.Left" Size="DrawerSize.Sm">
    <ChildContent>
        <p>Arama filtreleri buraya gelecektir...</p>
    </ChildContent>
    <Footer>
        <ThButton Variant="ButtonVariant.Ghost" OnClick="@(() => _drawerVisible = false)">Kapat</ThButton>
    </Footer>
</ThDrawer>
```

### 2. Dinamik Form/Servis Kullanımı (C# Form gibi sonuç değerleri ile)
```csharp
@inject ThDrawerService DrawerService

private async Task OpenFormAsync()
{
    var options = new DrawerOptions { Position = DrawerPosition.Right, Size = DrawerSize.Md, Expandable = true };
    var parameters = new Dictionary<string, object> { ["InitialValue"] = "Filtre Değeri" };
    
    var drawerRef = DrawerService.Show<ShowcaseDrawerContent>("Gelişmiş Arama", parameters, options);
    var result = await drawerRef.Result;
    
    if (!result.Cancelled)
    {
        var data = result.Data as string;
        // Sonuç verisiyle işlem yap (DrawerResult)...
    }
}
```

## Kullanılan token'lar
- `--th-overlay`
- `--th-surface`
- `--th-surface-2`
- `--th-border`
- `--th-radius-md`
- `--th-shadow-xl`
- `--th-text`
- `--th-text-muted`
- `--th-primary`
- `--th-success`
- `--th-danger`
- `--th-warning`
- `--th-info`
- `--th-transition`
- `--th-ease`

## Tema/uyum notları
- Yan panelin tüm boyutları (genişlik/yükseklik), kenarlıkları, renkleri ve başlık varyantları aktif temaya (`light` / `dark`) ve density/scale ayarlarına tam uyum sağlar.
- Panel açılış ve kapanış yönlerine göre tasarlanmış CSS `transform` animasyonları (`translateX` / `translateY`) ile akıcı bir kullanıcı deneyimi sunar.
- **Fare ile Genişletme (Drag Resize)**: `Expandable` özelliği `true` olduğunda, panelin kenarında kesikli bir sınır çizgisi (drag handle) belirir. Kullanıcı bu sınırı fare veya dokunmatik ekranlar üzerinden basılı tutup sürükleyerek paneli serbestçe genişletebilir veya daraltabilir.
- **Sayfa Kaydırma Kilidi (Scroll Lock)**: Yan panel açıldığında, sayfa arka planının kaydırılmasını önlemek amacıyla `document.body` üzerine `.th-no-scroll` sınıfı otomatik olarak uygulanır; panel kapatıldığında ise bu sınıf kaldırılır.
