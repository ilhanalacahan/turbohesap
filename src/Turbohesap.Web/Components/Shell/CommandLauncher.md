# CommandLauncher

> Uygulama genelinde klavye kısayolu (Ctrl + K) veya arama çubuğu üzerinden tetiklenen, sayfa arama ve hızlı komut çalıştırma sağlayan komut paleti bileşeni.

- **Sınıf**: `th-command`
- **Namespace**: `Turbohesap.Web.Components.Shell`
- **Dosyalar**: `CommandLauncher.razor`, `CommandLauncher.razor.cs`, `Frontend/css/command-launcher.css`

## Parametreler
Bu bileşenin doğrudan dışarıdan aldığı bir `[Parameter]` bulunmamaktadır. Görünürlük ve durum yönetimi `LayoutState` servisi üzerinden merkezi olarak koordine edilmektedir.

## Kullanım

Bileşen `MainLayout` içerisinde yer alır ve global durum değişikliklerini dinler:

```razor
<CommandLauncher />
```

Kullanıcı `Ctrl + K` kombinasyonuna bastığında veya üst çubuktaki arama alanına tıkladığında `LayoutState.CommandOpen` değeri `true` set edilerek görünür hale getirilir.

## Kullanılan token'lar
- `--th-z-command`
- `--th-overlay`
- `--th-transition`
- `--th-ease`
- `--th-surface`
- `--th-border`
- `--th-radius-xl`
- `--th-shadow-xl`
- `--th-text-subtle`
- `--th-text`
- `--th-primary-subtle`
- `--th-primary`
- `--th-radius-md`

## Tema/uyum notları
- **Yumuşak Geçiş (Fade-in)**: Açılış esnasında geriye kalan sayfa karartılarak diyalog pencerelerinde olduğu gibi yumuşak bir opaklık (`th-fade-in` / `opacity`) animasyonu uygulanır.
- **Koyu Tema Uyumluluğu**: Arka plan overlay rengi ve metinlerin zıtlığı, koyu tema etkinleştirildiğinde CSS token'ları vasıtasıyla gerçek zamanlı olarak güncellenir.
- **Odaklanma ve Navigasyon**: Arama alanı açıldığı anda otomatik odaklanma (`autofocus`) kazanır; `Escape` ile kapatılabilir.
