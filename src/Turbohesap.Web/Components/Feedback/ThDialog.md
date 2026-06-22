# ThDialogService (JS Interop Dialog)

> Sunucu gidiş-dönüş gecikmesini (roundtrip latency) ortadan kaldıran, tamamen istemci tarafında (TypeScript) oluşturulup C# event ve callbacks ile senkronize edilen yüksek performanslı dinamik diyalog motoru.

- **Servis**: `ThDialogService`
- **Namespace**: `Turbohesap.Web.Services`
- **Dosyalar**: `ThDialogService.cs`, `ThDialogRef.cs`, `Frontend/scripts/dialog.ts`, `Frontend/css/dialog.css`

## Metotlar
| Metot | Geri Dönüş Tipi | Açıklama |
|-------|-----------------|----------|
| `AlertAsync(title, message, variant)` | `Task<DialogResult>` | Basit bir uyarı/mesaj kutusu açar. |
| `SuccessAsync(title, message)` | `Task<DialogResult>` | Başarılı durum ikonu içeren mesaj kutusu açar. |
| `WarningAsync(title, message)` | `Task<DialogResult>` | Uyarı ikonlu mesaj kutusu açar. |
| `ErrorAsync(title, message)` | `Task<DialogResult>` | Hata ikonlu mesaj kutusu açar. |
| `ConfirmAsync(title, message, variant, confirmText, cancelText)` | `Task<DialogResult>` | Evet/Hayır (onay) kutusu açar. |
| `PromptAsync(title, message, placeholder, variant)` | `Task<DialogResult>` | Kullanıcıdan metin girdisi alan prompt kutusu açar. |
| `LoadingAsync(title, message)` | `Task<ThDialogRef>` | İptal edilemeyen/bloklayan spinner (yükleniyor) ekranı açar. |
| `BusyAsync(title, message)` | `Task<ThDialogRef>` | Meşgul durumda olduğunu belirten spinner ekranı açar. |
| `ProgressAsync(title, message, value)` | `Task<ThDialogRef>` | İlerleme çubuğu içeren diyalog açar (C# tarafında yüzde güncellenebilir). |
| `CountdownAsync(title, message, duration)` | `Task<ThDialogRef>` | Geri sayım sayacı içeren diyalog açar (C# tarafında saniye güncellenebilir). |

## Dialog Seçenekleri (DialogOptions)
- `Type`: Diyalog tipi (`Alert`, `Confirm`, `Prompt`, `Loading`, `Progress`, `Countdown`, `Success`, `Warning`, `Error`, `Busy`).
- `Variant`: Diyalog başlık ve onay butonu renk teması (`Normal`, `Primary`, `Success`, `Warning`, `Danger`, `Info`).
- `Title`: Başlık metni.
- `Message`: Mesaj gövdesi metni.
- `Placeholder`: Prompt için giriş alanı placeholder metni.
- `ConfirmText`: Onay buton yazısı (varsayılan: "Tamam").
- `CancelText`: İptal buton yazısı (varsayılan: "İptal").
- `Duration`: Geri sayım süresi (saniye).
- `Value`: İlerleme yüzdesi (0 - 100).

## Kullanım

### 1. Basit Onay Kutusu (Confirm)
```csharp
@inject ThDialogService DialogService
@inject ToastService Toasts

private async Task DeleteUserAsync()
{
    var result = await DialogService.ConfirmAsync(
        "Kullanıcıyı Sil", 
        "Seçili kullanıcı kaydını silmek istediğinize emin misiniz? Bu işlem geri alınamaz.", 
        DialogVariant.Danger, 
        "Evet, Sil", 
        "Vazgeç"
    );

    if (result.Status == DialogStatus.Confirmed)
    {
        Toasts.Success("Kullanıcı kaydı başarıyla silindi.");
    }
}
```

### 2. Metin Girişi (Prompt)
```csharp
var result = await DialogService.PromptAsync("Kategori Ekle", "Yeni kategori adını girin:");
if (result.Status == DialogStatus.Confirmed && !string.IsNullOrWhiteSpace(result.Value))
{
    // Kategori ekleme mantığı...
}
```

### 3. İlerleme Takibi (Progress - C# Callback ve Event Entegrasyonu)
```csharp
var dialogRef = await DialogService.ProgressAsync("Veriler Dışa Aktarılıyor", "Lütfen bekleyin...", 0);

for (int i = 0; i <= 100; i += 20)
{
    await Task.Delay(200); // sunucu işlemi
    await dialogRef.UpdateProgressAsync(i); // JS tarafındaki yüzdeyi günceller
}

await dialogRef.CloseAsync(); // Diyaloğu programatik olarak kapatır
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
- `--th-text-muted`
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

## Tema/uyum notları
- TypeScript tarafında oluşturulan DOM düğmeleri tamamen semantik CSS sınıflarını kullandığından, aktif tema (`light` / `dark`) ve density ayarları anında pencereye yansır.
