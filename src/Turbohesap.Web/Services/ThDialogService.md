# ThDialogService

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

## Varyantlar
Diyaloglar, `DESIGN.md` ve tema sistemine uygun şekilde `variant` parametresine göre soft renkli başlıklar (`--th-primary-subtle`, `--th-danger-subtle`, vb.) ve kenarlık renkleri kazanır:
- `Normal`: Standart gri başlık
- `Primary`: Soft mavi başlık
- `Success`: Soft yeşil başlık
- `Warning`: Soft turuncu başlık
- `Danger`: Soft kırmızı başlık
- `Info`: Soft açık mavi başlık

## İkon Tasarımı (Side-by-Side Layout)
Diyalogların tasarımı, sol tarafta durum ikonu (büyük boyutlu ve renklendirilmiş) ve sağ tarafta ise diyalog içeriği (başlık, mesaj, girdi alanları, progress bar ve countdown saati) olacak şekilde iki sütunlu (`flex gap-4 items-start`) estetik bir yerleşime sahiptir. İkonlar tip veya varyanta göre otomatik belirlenir.

## Kullanım Örnekleri

### 1. Basit Onay Kutusu (Confirm)
```csharp
@inject ThDialogService DialogService

var result = await DialogService.ConfirmAsync(
    "Müşteriyi Sil", 
    "Bu müşteri kaydını silmek istediğinize emin misiniz? Bu işlem geri alınamaz.", 
    DialogVariant.Danger, 
    "Evet, Sil", 
    "Vazgeç"
);

if (result.Status == DialogStatus.Confirmed)
{
    // Silme mantığı
}
```

### 2. Metin Girişi (Prompt)
```csharp
var result = await DialogService.PromptAsync("Kategori Ekle", "Yeni kategori adını girin:");
if (result.Status == DialogStatus.Confirmed && !string.IsNullOrWhiteSpace(result.Value))
{
    // Ekleme mantığı
}
```
