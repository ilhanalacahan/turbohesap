# ThDropdown & ThContextMenu

> Gelişmiş açılır menü (dropdown) ve sağ tık bağlam menüsü (context menu) bileşenleri. `fixed` konumlandırma (React Portal benzeri) ile taşma koruması, otomatik yön belirleme ve çok seviyeli alt menü (submenu) desteği sunar.

- **Sınıf**: `th-dropdown-menu`, `th-dropdown-item`, `th-dropdown-divider`, `th-dropdown-header`
- **Namespace**: `Turbohesap.Web.Components.Dropdown` ve `Turbohesap.Web.Components.ContextMenu`
- **Dosyalar**:
  - `Dropdown/ThDropdown.razor` ve `ThDropdown.razor.cs`
  - `Dropdown/ThDropdownItem.razor` ve `ThDropdownItem.razor.cs`
  - `Dropdown/ThDropdownSubmenu.razor` ve `ThDropdownSubmenu.razor.cs`
  - `Dropdown/ThDropdownDivider.razor`
  - `ContextMenu/ThContextMenu.razor` ve `ThContextMenu.razor.cs`
  - `Frontend/css/dropdown.css`
  - `Frontend/scripts/menu.ts`

---

## Parametreler (ThDropdown)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Trigger` | `RenderFragment?` | — | Açılır menüyü tetikleyecek buton veya element (tıklama ile tetiklenir). |
| `ChildContent` | `RenderFragment?` | — | Menü öğelerinin (`ThDropdownItem`, `ThDropdownSubmenu`, `ThDropdownDivider`) listesi. |

## Parametreler (ThDropdownItem)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | Öğe metni. |
| `Icon` | `string?` | — | Öğe solunda gösterilecek Font Awesome ikon sınıfı (örn. `fa-solid fa-edit`). |
| `Shortcut` | `string?` | — | Öğe sağında gösterilecek klavye kısayolu metni (örn. `Ctrl+P`). |
| `Disabled` | `bool` | `false` | true ise öğe pasifleştirilir ve tıklanamaz. |
| `Danger` | `bool` | `false` | true ise öğe yıkıcı bir işlem olduğunu belirtmek için kırmızı (danger) renkte gösterilir. |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | Öğe tıklandığında tetiklenecek olay. |

## Parametreler (ThDropdownSubmenu)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Header` | `string` | — | Alt menü tetikleyici öğesinin başlığı (zorunlu). |
| `Icon` | `string?` | — | Alt menü tetikleyici öğesinin solundaki ikon. |
| `Disabled` | `bool` | `false` | true ise alt menü açılamaz. |
| `ChildContent` | `RenderFragment?` | — | Alt menünün içerdiği menü öğeleri. |

## Parametreler (ThContextMenu)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `TargetZone` | `RenderFragment?` | — | Sağ tıklanarak bağlam menüsünün açılacağı hedef alan. |
| `Menu` | `RenderFragment?` | — | Bağlam menüsünde gösterilecek menü öğeleri. |

---

## Kullanım

### 1. Klasik Dropdown (Tetikleyici Buton ile)
```razor
<ThDropdown>
    <Trigger>
        <ThButton Variant="ButtonVariant.Outline" Icon="fa-solid fa-ellipsis-vertical">İşlemler</ThButton>
    </Trigger>
    <ChildContent>
        <div class="th-dropdown-header">İşlemler</div>
        <ThDropdownItem Icon="fa-solid fa-pen" Shortcut="Ctrl+E" OnClick="Edit">Düzenle</ThDropdownItem>
        <ThDropdownItem Icon="fa-solid fa-copy" OnClick="Copy">Kopyala</ThDropdownItem>
        <ThDropdownDivider />
        <ThDropdownItem Icon="fa-solid fa-trash" Danger="true" OnClick="Delete">Sil</ThDropdownItem>
    </ChildContent>
</ThDropdown>
```

### 2. Çok Seviyeli Alt Menülü Dropdown (Submenus)
```razor
<ThDropdown>
    <Trigger>
        <ThButton Variant="ButtonVariant.Primary">Dışa Aktar</ThButton>
    </Trigger>
    <ChildContent>
        <ThDropdownSubmenu Header="Veriyi Paylaş" Icon="fa-solid fa-share">
            <ThDropdownItem Icon="fa-brands fa-twitter" OnClick="ShareTwitter">Twitter</ThDropdownItem>
            <ThDropdownItem Icon="fa-brands fa-facebook" OnClick="ShareFacebook">Facebook</ThDropdownItem>
        </ThDropdownSubmenu>
        <ThDropdownSubmenu Header="Dosya Olarak İndir" Icon="fa-solid fa-download">
            <ThDropdownItem Icon="fa-solid fa-file-pdf" OnClick="DownloadPdf">PDF</ThDropdownItem>
            <ThDropdownItem Icon="fa-solid fa-file-excel" OnClick="DownloadExcel">Excel</ThDropdownItem>
        </ThDropdownSubmenu>
    </ChildContent>
</ThDropdown>
```

### 3. Sağ Tık Bağlam Menüsü (Context Menu)
```razor
<ThContextMenu>
    <TargetZone>
        <div class="p-8 border rounded text-center">
            Sağ tıklayarak menüyü açın
        </div>
    </TargetZone>
    <Menu>
        <ThDropdownItem Icon="fa-solid fa-rotate" OnClick="Refresh">Yenile</ThDropdownItem>
        <ThDropdownDivider />
        <ThDropdownSubmenu Header="Yeni Ekle" Icon="fa-solid fa-plus">
            <ThDropdownItem Icon="fa-solid fa-file" OnClick="CreateFile">Yeni Dosya</ThDropdownItem>
        </ThDropdownSubmenu>
    </Menu>
</ThContextMenu>
```

---

## Kullanılan token'lar
- `--th-surface` (Arka plan rengi)
- `--th-border` (Kenarlık rengi)
- `--th-radius-md` (Menü köşe yuvarlaklığı)
- `--th-radius-sm` (Menü öğesi köşe yuvarlaklığı)
- `--th-shadow-lg` (Menü kutusu gölgesi)
- `--th-text` (Birincil metin rengi)
- `--th-text-muted` (İkincil metin ve ikon rengi)
- `--th-text-subtle` (Kısayol ve başlık rengi)
- `--th-surface-3` (Hover zemin rengi)
- `--th-danger` (Yıkıcı işlem rengi)
- `--th-danger-subtle` (Yıkıcı işlem hover zemin rengi)
- `--th-transition-fast` (Hover animasyon geçişi)
- `--th-ease` (Animasyon ivmesi)

## Tema/uyum notları
- Menüler, açıldıkları yerin viewport (ekran) sınırlarına göre taşma kontrolü yapar. Eğer menü veya alt menü ekran sınırlarından taşarsa, otomatik olarak aksi yönde (örn. alta sığmıyorsa üste, sağa sığmıyorsa sola) konumlanacak biçimde akıllıca yer değiştirir.
- CSS `position: fixed` ve JS `getBoundingClientRect` ile konumlandırıldıkları için `overflow: hidden` olan herhangi bir ebeveyn kapsayıcı tarafından kesilmezler (React Portal benzeri davranış).
- Tüm renkler, gölgeler ve geçiş animasyonları aktif temaya (`light` / `dark`) anında uyum sağlar.
