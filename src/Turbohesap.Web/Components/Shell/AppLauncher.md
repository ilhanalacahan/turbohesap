# AppLauncher

> Uygulama modülleri arasında hızlı geçiş sağlayan, Microsoft 365 benzeri geniş ve ferah ızgara (grid) düzenine sahip uygulama başlatıcı bileşeni.

- **Sınıf**: `th-applauncher`
- **Namespace**: `Turbohesap.Web.Components.Shell`
- **Dosyalar**: `AppLauncher.razor`, `AppLauncher.razor.cs`, `Frontend/css/app-launcher.css`

## Parametreler
Bu bileşenin doğrudan dışarıdan aldığı bir `[Parameter]` bulunmamaktadır. Görünürlük ve durum yönetimi `LayoutState` servisi üzerinden merkezi olarak koordine edilmektedir.

## Kullanım

Bileşen `MainLayout` içerisinde yer alır ve global durum değişikliklerini dinler:

```razor
<AppLauncher />
```

Kullanıcı sol üst köşedeki uygulama başlatıcı ikonuna tıkladığında `LayoutState.AppLauncherOpen` değeri `true` set edilerek görünür hale getirilir.

## Kullanılan token'lar
- `--th-z-command`
- `--th-overlay`
- `--th-transition`
- `--th-ease`
- `--th-surface`
- `--th-border`
- `--th-radius-xl`
- `--th-shadow-xl`
- `--th-fw-semibold`
- `--th-text`
- `--th-text-muted`
- `--th-surface-2`
- `--th-radius-lg`
- `--th-transition-fast`
- `--th-border-strong`
- `--th-shadow-md`
- `--th-radius-full`
- `--th-surface-3`
- `--th-text-subtle`
- `--th-fw-medium`

## Tema/uyum notları
- **Yumuşak Geçiş (Fade-in)**: Açılış esnasında arka plan karartılarak diyalog pencerelerinde olduğu gibi yumuşak bir opaklık (`th-fade-in` / `opacity`) animasyonu uygulanır.
- **Koyu Tema Uyumluluğu**: Koyu tema etkinleştirildiğinde arayüz zeminleri, kenarlık renkleri ve kart gölgeleri CSS token'ları ile gerçek zamanlı olarak güncellenir.
- **Uygulama Kartları**: Kart simgeleri, her uygulamanın kendi vurgu rengini (`--accent`) `color-mix` yöntemiyle %14 opaklıkta kullanarak zemin rengiyle harmanlar.
