# ThToastHost & ToastService

> Çoklu pozisyon desteği, özel ikonlar, eylem butonları, otomatik/dinamik ilerleme çubuğu (progress bar) ve sabitlenebilirlik (fixed) özelliklerine sahip zengin bildirim sistemi.

- **Sınıf**: `th-toast-region` / `th-toast` (+ durum ve pozisyon varyantları)
- **Namespace**: `Turbohesap.Web.Components.Feedback` / `Turbohesap.Web.Services`
- **Dosyalar**: `ThToastHost.razor`, `ThToastHost.razor.cs`, `ToastService.cs`, `ToastMessage.cs`, `Frontend/css/toast.css`

## Parametreler (ToastMessage)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Message` | `string` | - | Bildirim gövde metni (zorunlu). |
| `Title` | `string?` | - | Bildirim başlığı. |
| `Level` | `ToastLevel` | `ToastLevel.Info` | Durum seviyesi (`Info`, `Success`, `Warning`, `Danger`). |
| `DurationMs` | `int` | `4000` | Kapanma süresi (milisaniye). |
| `Icon` | `string?` | - | Özel ikon sınıfı (örn. `fa-solid fa-cloud-arrow-up`). `none` verilirse ikon gizlenir. |
| `Position` | `ToastPosition` | `ToastPosition.BottomRight` | Ekrandaki konumu (`TopRight`, `TopLeft`, `BottomRight`, `BottomLeft`, `TopCenter`, `BottomCenter`). |
| `ShowProgress` | `bool` | `false` | Süre azalışını gösteren animasyonlu ilerleme çubuğunu açar. |
| `Progress` | `double?` | - | İsteğe bağlı elle yönetilen C# ilerleme yüzdesi (0 - 100). |
| `Fixed` | `bool` | `false` | true ise otomatik kapanmaz, ekranda sabit kalır. |
| `Actions` | `List<ToastAction>?` | - | Bildirim altına yerleştirilecek etkileşimli buton listesi. |

## Pozisyonlar
Farklı pozisyonlardaki toast bildirimleri kendi aralarında gruplanır ve ekranda bağımsız olarak üst üste yığılır:
- `.th-toast-region--top-right`
- `.th-toast-region--top-left`
- `.th-toast-region--bottom-right`
- `.th-toast-region--bottom-left`
- `.th-toast-region--top-center`
- `.th-toast-region--bottom-center`

## Eylem Butonları (ToastAction)
Bildirime eklenen eylem butonları (örn. "Geri Al" / "Undo" butonu) tıklandığında ilişkili C# callback fonksiyonunu tetikler ve bildirimi otomatik kapatır.

## Kullanım

### 1. Basit Başarılı Bildirimi
```csharp
@inject ToastService Toasts
Toasts.Success("Kullanıcı kaydı oluşturuldu.", "Başarılı");
```

### 2. Geri Al Butonlu ve Progress Çubuklu Bildirim
```csharp
var actions = new List<ToastAction>
{
    new() 
    { 
        Text = "Geri Al", 
        Icon = "fa-solid fa-rotate-left", 
        VariantClass = "th-btn--subtle",
        OnClick = () => RestoreDeletedItem()
    }
};

Toasts.Warning("Dosya silindi.", "Geri Alınabilir", durationMs: 8000, showProgress: true, actions: actions);
```

### 3. Farklı Pozisyonda Sabit (Fixed) Bildirim
```csharp
Toasts.Error("Sunucu bağlantısı koptu! Lütfen bekleyin.", "Bağlantı Hatası", isFixed: true, position: ToastPosition.TopCenter);
```

## Kullanılan token'lar
- `--th-overlay`
- `--th-surface`
- `--th-border`
- `--th-radius-md`
- `--th-shadow-lg`
- `--th-text`
- `--th-text-muted`
- `--th-text-subtle`
- `--th-primary`
- `--th-success`
- `--th-warning`
- `--th-danger`
- `--th-info`

## Tema/uyum notları
- Toast pencerelerinin tüm renkleri, köşeleri (`--th-radius-md`), gölgeleri ve dikey paddingleri aktif temaya göre anında değişir.
- Otomatik progress çubuğu animasyonu, kullanıcının fare imlecini bildirimin üzerine getirmesi durumunda geçici olarak duraklar (`animation-play-state: paused`).
