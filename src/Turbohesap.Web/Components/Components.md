# Bileşen Kataloğu — Components.md

Bu dosya, Turbohesap arayüz bileşenlerinin **dökümantasyon indeksidir**. Her bileşenin ayrıntılı
dökümanı kendi dizinindeki `Th<Ad>.md` dosyasındadır. Bu liste `create-component` ve
`update-component` becerileri tarafından güncel tutulur.

> **Durum**: ✅ belge yazıldı · ⏳ belge beklemede (update-component ile yazılacak)

## Yeniden kullanılan UI bileşenleri (`Th*`)

| Bileşen | Belge | Özet | Durum |
|---------|-------|------|-------|
| ThButton | [Button/ThButton.md](Button/ThButton.md) | Birincil/ikincil/ghost… varyantlı buton (`th-btn`) | ⏳ |
| ThField | [Input/ThField.md](Input/ThField.md) | Etiket + içerik + FluentValidation mesajı form alanı | ⏳ |
| ThModal | [Feedback/ThModal.md](Feedback/ThModal.md) | Taşınabilir, tam ekran, genişletilebilir ve form davranışlı modal pencere | ✅ |
| ThDialog | [Feedback/ThDialog.md](Feedback/ThDialog.md) | Sıfır gecikmeli, TS-interop tabanlı dinamik diyalog ve bildirim servisi | ✅ |
| ThToastHost | [Feedback/ThToastHost.md](Feedback/ThToastHost.md) | Toast bölgesi (otomatik kapanır) | ✅ |
| ThDrawer | [Feedback/ThDrawer.md](Feedback/ThDrawer.md) | Dört yönden açılabilen, genişletilebilir ve form davranışlı yan panel | ✅ |
| ThTabs | [Tabs/ThTabs.md](Tabs/ThTabs.md) | Çok stilli, carousel kaydırmalı ve onay olaylı sekme bileşeni | ✅ |
| ThDropdown | [Dropdown/ThDropdown.md](Dropdown/ThDropdown.md) | Alt menülü, taşma korumalı ve portal-benzeri açılır menü bileşeni | ✅ |
| ThContextMenu | [Dropdown/ThDropdown.md](Dropdown/ThDropdown.md) | Sağ tık ile tetiklenen bağlam menüsü bileşeni | ✅ |
| ThErrorBoundary | [Feedback/ThErrorBoundary.md](Feedback/ThErrorBoundary.md) | Hata sınırı; hatayı API'ye loglar | ⏳ |
| ThDataTable | [Data/ThDataTable.md](Data/ThDataTable.md) | Generic veri tablosu (yuvalar + yükleniyor/boş) | ⏳ |
| ThPagination | [Data/ThPagination.md](Data/ThPagination.md) | Sayfalama çubuğu (parametrik sayfa boyutu) | ⏳ |

## Yerleşim bileşenleri (`Layout`)

| Bileşen | Belge | Özet | Durum |
|---------|-------|------|-------|
| ThPage | [Layout/ThPage.md](Layout/ThPage.md) | Sayfa sarmalayıcı; PageTabs kaydı + kirli tutamaç | ⏳ |
| MainLayout | [Layout/MainLayout.md](Layout/MainLayout.md) | Kimlikli kabuk + hata sınırı | ⏳ |
| AuthLayout | [Layout/AuthLayout.md](Layout/AuthLayout.md) | Kimlik sayfaları nötr kabuğu | ⏳ |

## Kabuk bileşenleri (`Shell`)

| Bileşen | Belge | Özet | Durum |
|---------|-------|------|-------|
| Sidebar | [Shell/Sidebar.md](Shell/Sidebar.md) | Menü ağacı + daraltılmış flyout | ⏳ |
| AppBar | [Shell/AppBar.md](Shell/AppBar.md) | Üst bar (breadcrumb, arama, aksiyonlar) | ⏳ |
| PageTabs | [Shell/PageTabs.md](Shell/PageTabs.md) | Sayfa sekmeleri + kirli durum + kapatma onayı | ⏳ |
| CommandLauncher | [Shell/CommandLauncher.md](Shell/CommandLauncher.md) | Komut paleti (Ctrl K) | ⏳ |
| AppLauncher | [Shell/AppLauncher.md](Shell/AppLauncher.md) | Uygulama başlatıcı ızgarası | ⏳ |
| ThemeDesigner | [Shell/ThemeDesigner.md](Shell/ThemeDesigner.md) | Tema tasarımcısı drawer'ı | ⏳ |
| AiChat | [Shell/AiChat.md](Shell/AiChat.md) | AI asistan paneli | ⏳ |

---

Yeni bileşenler `create-component` ile eklenir; her ekleme bu tabloya bir satır yazar ve bileşenin
dizininde `Th<Ad>.md` oluşturur. Var olan bileşenlerin dökümanları `update-component` ile yazılır.
