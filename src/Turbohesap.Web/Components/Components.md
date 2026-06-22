# Bileşen Kataloğu — Components.md

Bu dosya, Turbohesap arayüz bileşenlerinin **dökümantasyon indeksidir**. Her bileşenin ayrıntılı
dökümanı kendi dizinindeki `Th<Ad>.md` dosyasındadır. Bu liste `create-component` ve
`update-component` becerileri tarafından güncel tutulur.

> **Durum**: ✅ belge yazıldı · ⏳ belge beklemede (update-component ile yazılacak)

## Yeniden kullanılan UI bileşenleri (`Th*`)

| Bileşen | Belge | Özet | Durum |
|---------|-------|------|-------|
| ThButton | [Button/ThButton.md](Button/ThButton.md) | Birincil/ikincil/ghost… varyantlı buton (`th-btn`) | ⏳ |
| ThBadge | [Badge/ThBadge.md](Badge/ThBadge.md) | Durum renkli ve gösterge noktalı rozet bileşeni | ✅ |
| ThAvatar | [Avatar/ThAvatar.md](Avatar/ThAvatar.md) | Kullanıcı profil resmi, baş harfler ve mevcudiyet durum göstergeli avatar | ✅ |
| ThCard | [Card/ThCard.md](Card/ThCard.md) | Başlık, gövde ve altbilgi şeritli genel sarmalayıcı kart paneli | ✅ |
| ThProgress | [Progress/ThProgress.md](Progress/ThProgress.md) | Durum renkli, etiketli ve yüzde göstergeli ilerleme çubuğu | ✅ |
| ThTooltip | [Tooltip/ThTooltip.md](Tooltip/ThTooltip.md) | Hover/focus durumlarında açılan ipucu bilgilendirme kutusu | ✅ |
| ThPopover | [Popover/ThPopover.md](Popover/ThPopover.md) | Tıklamayla açılan, form/buton barındıran gelişmiş balon pencere | ✅ |
| ThAlert | [Alert/ThAlert.md](Alert/ThAlert.md) | Inline uyarı, başarı, hata ve bilgi panelleri | ✅ |
| ThBreadcrumb | [Breadcrumb/ThBreadcrumb.md](Breadcrumb/ThBreadcrumb.md) | Sayfa konum hiyerarşisi gösteren ekmek kırıntısı patikası | ✅ |
| ThSpinner | [Spinner/ThSpinner.md](Spinner/ThSpinner.md) | İşlem sürerken gösterilen dairesel yükleniyor animasyonu | ✅ |
| ThField | [Input/ThField.md](Input/ThField.md) | Etiket + içerik + FluentValidation mesajı form alanı | ⏳ |
| ThSwitch | [Input/ThSwitch.md](Input/ThSwitch.md) | Erişilebilir ve özelleştirilmiş anahtar (switch) bileşeni | ✅ |
| ThCheckbox | [Input/ThCheckbox.md](Input/ThCheckbox.md) | Klavye ve ikon uyumlu özelleştirilmiş seçim kutusu (checkbox) bileşeni | ✅ |
| ThTextArea | [Input/ThTextArea.md](Input/ThTextArea.md) | Çok satırlı metin giriş kutusu (textarea) bileşeni | ✅ |
| ThModal | [Feedback/ThModal.md](Feedback/ThModal.md) | Taşınabilir, tam ekran, genişletilebilir ve form davranışlı modal pencere | ✅ |
| ThDialog | [Feedback/ThDialog.md](Feedback/ThDialog.md) | Sıfır gecikmeli, TS-interop tabanlı dinamik diyalog ve bildirim servisi | ✅ |
| ThToastHost | [Feedback/ThToastHost.md](Feedback/ThToastHost.md) | Toast bölgesi (otomatik kapanır) | ✅ |
| ThDrawer | [Feedback/ThDrawer.md](Feedback/ThDrawer.md) | Dört yönden açılabilen, genişletilebilir ve form davranışlı yan panel | ✅ |
| ThTabs | [Tabs/ThTabs.md](Tabs/ThTabs.md) | Çok stilli, dikey/yatay yönlü, carousel kaydırmalı ve onay olaylı sekme bileşeni | ✅ |
| ThAccordion | [Accordion/ThAccordion.md](Accordion/ThAccordion.md) | Tek/çoklu açılımlı, üç varyantlı açılır içerik grupları (`th-accordion`) | ✅ |
| ThDropdown | [Dropdown/ThDropdown.md](Dropdown/ThDropdown.md) | Alt menülü, taşma korumalı ve portal-benzeri açılır menü bileşeni | ✅ |
| ThContextMenu | [Dropdown/ThDropdown.md](Dropdown/ThDropdown.md) | Sağ tık ile tetiklenen bağlam menüsü bileşeni | ✅ |
| ThErrorBoundary | [Feedback/ThErrorBoundary.md](Feedback/ThErrorBoundary.md) | Hata sınırı; hatayı API'ye loglar | ⏳ |
| ThDataTable | [Data/ThDataTable.md](Data/ThDataTable.md) | Generic veri tablosu (yuva/deklaratif kolon, seçim, sıralama, sayfalama, kolon seçimi, satır olayları) | ✅ |
| ThQueryBuilder | [QueryBuilder/ThQueryBuilder.md](QueryBuilder/ThQueryBuilder.md) | AI agents ve web servisleri uyumlu dinamik filtre oluşturucu | ✅ |
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
| CommandLauncher | [Shell/CommandLauncher.md](Shell/CommandLauncher.md) | Komut paleti (Ctrl K) | ✅ |
| AppLauncher | [Shell/AppLauncher.md](Shell/AppLauncher.md) | Uygulama başlatıcı ızgarası | ✅ |
| ThemeDesigner | [Shell/ThemeDesigner.md](Shell/ThemeDesigner.md) | Tema tasarımcısı drawer'ı | ⏳ |
| AiChat | [Shell/AiChat.md](Shell/AiChat.md) | AI asistan paneli | ⏳ |

---

Yeni bileşenler `create-component` ile eklenir; her ekleme bu tabloya bir satır yazar ve bileşenin
dizininde `Th<Ad>.md` oluşturur. Var olan bileşenlerin dökümanları `update-component` ile yazılır.
