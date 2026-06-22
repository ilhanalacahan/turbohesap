# DESIGN.md — Turbohesap Tasarım Sistemi

Bu belge Turbohesap arayüz tasarım sistemini eksiksiz tanımlar. Amaç: **yalnızca bu belgeyi kullanarak sistemin birebir aynısını kurabilmek.** Her parametre token'dır; her bileşen token'lara bağlıdır. Token değişince ona bağlı her şey otomatik güncellenir.

İlkeler:
- **Token-first**: Hiçbir bileşen sabit renk/ölçü içermez. Tek doğruluk kaynağı `Frontend/themes/tokens.css`.
- **Mobile-first**: Önce mobil; `min-width` kırılımlarıyla genişler.
- **Kurumsal & sade**: Yüksek okunabilirlik, ölçülü gölge/animasyon, tutarlı boşluk ritmi.
- **Açık sınıf adları**: Bileşen sınıfları (`th-btn--primary`) ve Tailwind utility'leri **tam** yazılır; string interpolasyonu **yasak**.

---

## 1. Derleme hattı (build pipeline)

Tüm frontend kaynağı `src/Turbohesap.Web/Frontend/` altındadır ve **Vite + Tailwind v4 + TypeScript** ile derlenir.

```
Frontend/
├─ package.json            # vite, tailwindcss, @tailwindcss/vite, typescript, @fortawesome/fontawesome-free
├─ vite.config.ts          # çıktı adları + font ayrıştırma
├─ tsconfig.json
├─ main.ts                 # giriş: main.css + scriptleri toplar
├─ main.css                # giriş CSS: tailwind + @theme + @source + tüm @import'lar
├─ themes/
│  ├─ tokens.css           # ★ tüm tasarım tokenları (açık + koyu)
│  └─ base.css             # reset, gövde, scrollbar, odak halkası, ölçek
├─ css/                    # bileşen başına bir dosya
│  ├─ button.css input.css card.css badge.css dialog.css drawer.css toast.css datatable.css
│  └─ sidebar.css appbar.css layout.css tabs.css command-launcher.css app-launcher.css ai-chat.css
└─ scripts/
   ├─ theme.ts             # tema modu + token override
   └─ interop.ts           # window.turbohesap API (Blazor JS interop)
```

**Çıktı** (`vite build` → `make front`):
- `wwwroot/turbohesap.css` — Tailwind + tokenlar + tüm bileşen stilleri + Font Awesome CSS
- `wwwroot/turbohesap.js` — tema + interop (ES module)
- `wwwroot/fonts/*.woff2` — Font Awesome webfont'ları **ayrı** derlenir

`Components/App.razor` bunları `<head>`'de yükler; `turbohesap.js` FOUC'u önlemek için erken çalışır.

---

## 2. Token sistemi

Tokenlar `:root` (açık) ve `[data-theme="dark"]` (koyu) altında tanımlıdır. **Yapısal** tokenlar (yarıçap, ölçü, tipografi, hareket, z-index) moddan bağımsızdır; **renk** tokenları her mod için ayrıdır.

### 2.1 Renkler (semantik)

| Token | Açık | Koyu | Kullanım |
|-------|------|------|----------|
| `--th-bg` | `#f5f6f8` | `#0b1016` | Uygulama arka planı |
| `--th-surface` | `#ffffff` | `#131a23` | Kart/panel yüzeyi |
| `--th-surface-2` | `#f8f9fb` | `#1a232e` | İkincil yüzey (toolbar, footer) |
| `--th-surface-3` | `#eef1f4` | `#212c39` | Hover/girinti yüzeyi |
| `--th-overlay` | `rgba(15,23,42,.45)` | `rgba(0,0,0,.62)` | Modal/drawer arka karartma |
| `--th-border` | `#e4e7eb` | `#283341` | Standart kenarlık |
| `--th-border-strong` | `#cdd3da` | `#3a4757` | Belirgin kenarlık (input) |
| `--th-ring` | `rgba(37,99,235,.45)` | `rgba(59,130,246,.55)` | Odak halkası |
| `--th-text` | `#1a2230` | `#e6eaf0` | Birincil metin |
| `--th-text-muted` | `#586073` | `#9aa6b5` | İkincil metin |
| `--th-text-subtle` | `#8b93a3` | `#6b7888` | Soluk metin / placeholder |
| `--th-text-inverted` | `#ffffff` | `#0b1016` | Koyu zemin üzeri metin |
| `--th-primary` | `#2563eb` | `#3b82f6` | Marka / birincil eylem |
| `--th-primary-hover` / `-active` | `#1d4ed8` / `#1e40af` | `#60a5fa` / `#2563eb` | Durumlar |
| `--th-primary-fg` | `#ffffff` | `#ffffff` | Primary üzeri metin |
| `--th-primary-subtle` | `#e8f0fe` | `rgba(59,130,246,.16)` | Yumuşak primary zemin |
| `--th-accent` | `#7c3aed` | `#a78bfa` | Vurgu |
| `--th-success/-fg/-subtle` | `#16a34a` / `#fff` / `#e7f6ec` | `#22c55e` / … | Olumlu durum |
| `--th-warning/-fg/-subtle` | `#d97706` / … | `#f59e0b` / … | Uyarı |
| `--th-danger/-fg/-subtle` | `#dc2626` / … | `#ef4444` / … | Hata / yıkıcı eylem |
| `--th-info/-fg/-subtle` | `#0891b2` / … | `#06b6d4` / … | Bilgi |

**Yerleşim bölgeleri** (kendi tokenları, böylece kabuk ayrı temalanabilir):
`--th-sidebar-bg`, `--th-sidebar-fg`, `--th-sidebar-muted`, `--th-sidebar-active`, `--th-sidebar-border`, `--th-appbar-bg`, `--th-appbar-fg`, `--th-appbar-border`. (Kenar çubuğu açık temada da koyu/premium kalır.)

### 2.2 Yarıçap (radius)
`--th-radius-xs 4px` · `sm 6px` · `md 8px` · `lg 12px` · `xl 16px` · `2xl 24px` · `full 9999px`

### 2.3 Gölge (shadow)
`--th-shadow-xs/sm/md/lg/xl` — açık temada nötr; koyu temada daha derin (rgba siyah). Kartlar `xs`, açılır menüler `lg`, modal/drawer `xl` kullanır.

### 2.4 Tipografi
- `--th-font-sans`: Inter → system-ui zinciri · `--th-font-mono`: JetBrains Mono → ui-monospace
- Ağırlık: `--th-fw-normal 400`, `-medium 500`, `-semibold 600`, `-bold 700`
- Boyutlar Tailwind `text-*` ölçeğinden (xs … 2xl) gelir.

### 2.5 Yerleşim ölçüleri
| Token | Değer | Anlam |
|-------|-------|-------|
| `--th-sidebar-width` | `16rem` | Açık sidebar |
| `--th-sidebar-collapsed-width` | `4rem` | Daraltılmış sidebar |
| `--th-appbar-height` | `3.5rem` | Üst bar yüksekliği |
| `--th-footer-height` | `2.75rem` | Footer = sidebar footer ile **aynı** |
| `--th-tabbar-height` | `2.75rem` | Sekme şeridi |
| `--th-content-padding` / `-lg` | `1rem` / `1.5rem` | İçerik boşluğu (mobil/masaüstü) |
| `--th-control-height` / `-sm` / `-lg` | `2.5/2/3rem` | Düğme & input yüksekliği |

### 2.6 Hareket & z-index
- `--th-transition-fast 120ms`, `--th-transition 200ms`, `--th-transition-slow 320ms`, `--th-ease cubic-bezier(.4,0,.2,1)`
- `--th-z-sticky 100` < `dropdown 1000` < `drawer 1100` < `modal 1200` < `toast 1300` < `tooltip 1400` < `command 1500`

### 2.7 Global ölçek
`--th-scale` (vrsayılan `1`). `html { font-size: calc(var(--th-font-size-base) * var(--th-scale)) }`. Ölçek değişince rem tabanlı her boyut orantılı büyür/küçülür.

---

## 3. Tema mekanizması

- **Mod**: `<html data-theme="light|dark">`. `theme.ts` modu `localStorage`'da saklar; `system` modu `prefers-color-scheme`'i izler.
- **Çalışma zamanı özelleştirme**: `theme.applyTokens({ '--th-primary': '#…' })` ilgili CSS değişkenini `documentElement` üzerinde geçersiz kılar ve saklar; `resetTokens()` geri alır. `Settings/Theme.razor` sayfası bunu kullanır (renk, yarıçap, ölçek).
- **JS API** (`window.turbohesap.theme`): `setMode`, `toggle`, `getMode`, `current`, `applyTokens`, `resetTokens`. Blazor'dan `ThemeInterop` ile çağrılır.

---

## 4. Tailwind v4 entegrasyonu

`main.css`:
```css
@import 'tailwindcss';
@source '../Components/**/*.razor';   /* utility'ler .razor'dan taranır */
@source '../Pages/**/*.razor';
@custom-variant dark (&:where([data-theme='dark'], [data-theme='dark'] *));
@import './themes/tokens.css';
@theme inline { --color-primary: var(--th-primary); --radius-md: var(--th-radius-md); … }
```

- `@theme inline` Tailwind utility'lerini **runtime token'lara** bağlar: `bg-primary`, `text-text-muted`, `rounded-md`, `shadow-lg`, `bg-success-subtle` … hepsi `--th-*`'ye işaret eder; mod değişince güncellenir.
- Bileşen sınıfları (`th-*`) `@layer components`'tedir; markup'taki Tailwind utility'leri bunları geçersiz kılabilir.
- **Kural**: Sınıf adları her zaman tam yazılır. `class="@($"p-{x}")"` **yasak**; bunun yerine sabit sınıf eşlemesi (C# `switch` → `"th-btn--sm"`) kullanılır (bkz. `TurboComponentBase.SizeClass`).

---

## 5. Yerleşim anatomisi

```
┌───────────────────────────────────────────────────────────────┐
│ SIDEBAR (fixed)      │ APPBAR  [☰ breadcrumb] [ara] [🎨 🔔 ◐ 👤] │
│ ┌──────────────────┐ ├───────────────────────────────────────── │
│ │ [app] Turbohesap ☰│ │ TABBAR  [● Müşteriler ✕][Raporlar ✕]     │
│ ├──────────────────┤ ├───────────────────────────────────────── │
│ │ 🔎 menüde ara…    │ │ CONTENT (kaydırılır)                     │
│ │ GENEL            │ │  ┌─ th-page ──────────────────────────┐  │
│ │  • Gösterge      │ │  │ Başlık            [aksiyon ▾]       │  │
│ │ CARİ             │ │  │ … sayfa içeriği …                   │  │
│ │  • Müşteriler    │ │  └────────────────────────────────────┘  │
│ │  ▸ Raporlar      │ │  [opsiyonel footer — sidebar footer ile  │
│ ├──────────────────┤ │   aynı yükseklik]                        │
│ │ ? Yardım  💬 G.B. │ │                                    [🤖]  │
│ └──────────────────┘ │                                          │
└───────────────────────────────────────────────────────────────┘
```

- **Sidebar** (`th-shell` içinde): masaüstünde `sticky`, mobilde off-canvas (`th-sidebar--open` + `th-backdrop`). Daraltılabilir (`th-sidebar--collapsed` → yalnız ikonlar). Başlıkta solda **app ikonu** (tıklayınca AppLauncher), ortada marka, sağda hamburger. Altta **Yardım + Geri Bildirim**.
- **AppBar** (`sticky`): solda hamburger (mobil) + **breadcrumb**, ortada **arama** (tıklayınca CommandLauncher), sağda tema (🎨), bildirim (🔔), gece/gündüz (◐), kullanıcı (👤 → çıkış).
- **TabBar**: açık sekmeler. Menüden bir sayfa açıldığında sekme yoksa açılır, varsa odaklanır (`TabService`). Sekmesiz bağımsız sayfalar da olabilir.
- **İçerik**: `th-content` (kaydırma) → her sayfa `ThPage` ile sarılır. Normalde kenar boşluğu; harita gibi tam-ekran için `Flush`.
- **Footer**: `ThPage`'in opsiyonel `Footer` yuvası; `FooterFixed` ile sabitlenir; yükseklik sidebar footer ile aynıdır.
- **AI**: sağ altta sabit `th-ai-fab`; tıklayınca `th-ai-panel`.

---

## 6. Ortak başlık ve sekme stilleri

- **Panel başlığı** (`th-panel-header` + `th-panel-title` / `th-panel-subtitle`): kart, dialog, drawer ve accordion **aynı** başlık stilini kullanır → tutarlılık.
- **Footer şeritleri** (`th-card__footer`, `th-dialog__footer`, `th-drawer__footer`): `surface-2` zemin, üst kenarlık, sağa hizalı eylemler.
- **Sekme** (`th-tab`): pasif `text-muted`; aktif `th-tab--active` (alt 2px primary çizgi + `bg`). Kapatma `th-tab__close`.
- **Accordion** (`th-accordion`): `th-accordion__trigger` (panel başlığı dili), açılınca `th-accordion__item--open` + chevron 90° döner.

---

## 7. Bileşen kataloğu

Her bileşenin temel sınıfı `th-<ad>`; varyant `th-<ad>--<varyant>`; boyut `th-<ad>--sm|md|lg`. Blazor sarmalayıcıları `Components/` altındadır ve `TurboComponentBase`'den türer.

### 7.1 Button — `th-btn`
- Varyant: `--primary` `--secondary` `--outline` `--ghost` `--subtle` `--danger` `--success`
- Boyut: `--sm` `--md`(vrs.) `--lg`; ek: `--block`, `--icon`, `--loading`
- Blazor: `<ThButton Variant="ButtonVariant.Primary" Size="ComponentSize.Sm" Icon="fa-solid fa-plus" Loading Block />`

### 7.2 Form — `th-field`, `th-label`, `th-input`/`th-textarea`/`th-select`, `th-switch`
- Durumlar: odak (primary kenar + ring), `--error` (danger kenar), `:disabled`
- Yardım/hata: `th-hint`, `th-error`; ikonlu alan: `th-input-group` + `th-input-group__icon`
- Blazor: `<ThField TValue="string" Label="E-posta" Required For="() => model.Email"><InputText class="th-input" @bind-Value="model.Email" /></ThField>` — doğrulama mesajı `For`'dan gelir (FluentValidation).

### 7.3 Card / Panel — `th-card` (+ `th-panel-header`, `th-card__body`, `th-card__footer`)
### 7.4 Badge — `th-badge` + `--neutral|primary|success|warning|danger|info` (+ `th-badge__dot`)
### 7.5 Dialog / Modal — `th-overlay` + `th-dialog` (`--sm|md|lg|xl`)
- Mobile-first: tam genişlik alta yapışık; `sm`+ ortalanır. `ThDialog @bind-Visible` + `ChildContent`/`Footer` yuvaları.
### 7.6 Drawer — `th-drawer-overlay--right|left` + `th-drawer`
### 7.7 Toast — `th-toast-region` + `th-toast` (+ `--success|warning|danger|info`). `ToastService.Success/Error/...`; `ThToastHost` layout'ta bir kez render edilir, otomatik kapanır.
### 7.8 DataTable — `th-table-wrap` (toolbar + `th-table-scroll`/`th-table` + footer)
- `th-th--sortable` sıralanabilir başlık; `th-table__empty`/`__loading` durumları.
- Blazor: `<ThDataTable TItem="…" Items Loading ColumnCount>` yuvalar: `Toolbar`, `Header`, `Row`, `Footer`.
### 7.9 Pagination — `th-pagination` (bilgi + sayfa boyutu seçimi + ileri/geri). `<ThPagination Page PageSize TotalCount TotalPages PageChanged PageSizeChanged />`. Sayfa boyutu parametrik (10/20/50/100).

### 7.10 Kabuk bileşenleri
`Sidebar`, `AppBar`, `TabBar`, `CommandLauncher` (`th-command`), `AppLauncher` (`th-applauncher` + `th-app-tile`), `AiChat` (`th-ai-fab` + `th-ai-panel`). Durum koordinasyonu `LayoutState` servisidir.

---

## 8. İkonografi

**Font Awesome (Free)**. Sınıflar tam yazılır: `fa-solid fa-users`, `fa-regular fa-bell`, `fa-brands fa-…`. Webfont dosyaları `wwwroot/fonts/` altına ayrı derlenir. Yeni ikon eklemek için kod yeterli; ek yapılandırma gerekmez.

---

## 9. Erişilebilirlik & responsive

- Tüm etkileşimli öğelerde görünür odak halkası (`--th-ring`).
- `prefers-reduced-motion` ile animasyonlar kapatılır (`base.css`).
- Modal/drawer'da odak tuzağı (`turbohesap.focusTrap`).
- Kırılımlar: `640px (sm)`, `768px (md)`, `1024px (lg)`. Sidebar `md`'de sabitlenir; tablolar yatay kayar; toast `sm`'de sağ alta geçer.

---

## 10. Yeni bileşen ekleme kuralları

1. Stil → `Frontend/css/<ad>.css`; yalnızca `--th-*` token'ları kullan, sabit değer yazma.
2. `main.css`'e `@import './css/<ad>.css' layer(components);` ekle.
3. Blazor sarmalayıcı → `Components/<Ad>/Th<Ad>.razor`, `TurboComponentBase`'den türet; boyut/varyantı **sabit sınıf** eşlemesiyle ver (interpolasyon yok).
4. Açık + koyu modu test et (token'lara bağlı kaldıysan otomatik çalışır).
5. Bu kataloğa bir satır ekle.
