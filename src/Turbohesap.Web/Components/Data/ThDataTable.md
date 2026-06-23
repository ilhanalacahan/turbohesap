# ThDataTable / ThDataTableColumn

> Generic veri tablosu; yuva (Header/Row) veya deklaratif kolon modu, seçim, sıralama, sayfalama, kolon seçimi ve satır tıklama olayları.

- **Sınıf**: `th-table-wrap`, `th-table`, `th-table__row` (+ `--selected`, `--clickable`), `th-th--sortable`, `th-th--sorted`, `th-column-selector`
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThDataTable.razor`, `ThDataTable.razor.cs`, `ThDataTableColumn.razor`, `ThDataTableColumn.razor.cs`, `DataTableSelectionMode.cs`, `DataTableSortInfo.cs`, `Frontend/css/datatable.css`

## ThDataTable parametreleri
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Items` | `IReadOnlyList<TItem>` | `[]` | Satırlar |
| `Header` | `RenderFragment?` | — | Manuel başlık satırı (yuva modu) |
| `Row` | `RenderFragment<TItem>?` | — | Manuel veri satırı (yuva modu) |
| `Toolbar` | `RenderFragment?` | — | Araç çubuğu içeriği |
| `Footer` | `RenderFragment?` | — | Alt şerit; verilmezse yerleşik sayfalama gösterilebilir |
| `Columns` | `RenderFragment?` | — | Deklaratif kolonlar (`ThDataTableColumn`) |
| `Loading` | `bool` | `false` | Yükleniyor durumu |
| `EmptyText` | `string` | `"Kayıt bulunamadı."` | Boş durum metni |
| `ColumnCount` | `int` | `100` | Yükleniyor/boş satır colspan (yuva modu) |
| `SelectionMode` | `DataTableSelectionMode` | `None` | `None`/`Single`/`Multiple` |
| `SelectedItems` | `HashSet<TItem>` | `[]` | Seçili satırlar (`@bind-SelectedItems`) |
| `SelectedItemsChanged` | `EventCallback<HashSet<TItem>>` | — | Seçim değişti |
| `SelectOnRowClick` | `bool` | `false` | Satır tıklaması seçimi değiştirir |
| `OnRowClick` | `EventCallback<TItem>` | — | Satır tıklandı |
| `OnRowDoubleClick` | `EventCallback<TItem>` | — | Satır çift tıklandı |
| `SortKey` | `string?` | — | Aktif sıralama anahtarı (`@bind-SortKey` destekler) |
| `SortKeyChanged` | `EventCallback<string?>` | — | Sıralama anahtarı değiştiğinde tetiklenir |
| `SortDirection` | `SortDirection` | `SortDirection.Ascending` | Aktif sıralama yönü (`@bind-SortDirection` destekler) |
| `SortDirectionChanged` | `EventCallback<SortDirection>` | — | Sıralama yönü değiştiğinde tetiklenir |
| `SortChanged` | `EventCallback<DataTableSortInfo>` | — | Sıralama başlığı tıklandı |
| `Page` | `int?` | — | Yerleşik sayfalama: sayfa |
| `PageSize` | `int?` | — | Yerleşik sayfalama: sayfa boyutu |
| `TotalCount` | `int?` | — | Toplam kayıt |
| `TotalPages` | `int?` | — | Toplam sayfa |
| `PageChanged` | `EventCallback<int>` | — | Sayfa değişti |
| `PageSizeChanged` | `EventCallback<int>` | — | Sayfa boyutu değişti |
| `PageSizeOptions` | `int[]` | `[10,20,50,100]` | Sayfa boyutu seçenekleri |
| `ShowColumnSelector` | `bool` | `false` | Kolon görünürlük seçicisi |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## ThDataTableColumn parametreleri
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Title` | `string` | `""` | Kolon başlığı |
| `SortKey` | `string?` | — | Sıralama anahtarı; doluysa başlık tıklanabilir |
| `Sortable` | `bool` | `true` | Sıralama etkin mi |
| `Property` | `Expression<Func<TItem, object?>>?` | — | Hücre değeri (Template yoksa) |
| `ChildContent` | `RenderFragment<TItem>?` | — | Varsayılan özel hücre şablonu (çocuk etiket olarak) |
| `Template` | `RenderFragment<TItem>?` | — | Özel hücre şablonu (Geriye dönük uyumluluk için korunmuştur) |
| `Width` | `string?` | — | `width` stili (ör. `12rem`) |
| `ShowInSelector` | `bool` | `true` | Kolon seçicide listelensin mi |
| `Visible` | `bool` | `true` | Görünür mü (`@bind-Visible` desteklenir) |

## Kullanım

### Deklaratif kolon modu
```razor
<ThDataTable TItem="ProductDto" Items="_products"
             SelectionMode="DataTableSelectionMode.Multiple"
             @bind-SelectedItems="_selected"
             @bind-SortKey="_sortKey" @bind-SortDirection="_sortDir" SortChanged="OnSort"
             ShowColumnSelector="true"
             Page="_page" PageSize="_pageSize" TotalCount="_total" TotalPages="_pages"
             PageChanged="p => _page = p" PageSizeChanged="s => _pageSize = s"
             OnRowClick="OnRowClick" SelectOnRowClick="true">
    <Toolbar>
        <ThButton Variant="ButtonVariant.Primary" Icon="fa-solid fa-plus">Yeni</ThButton>
    </Toolbar>
    <Columns>
        <ThDataTableColumn TItem="ProductDto" Title="Kod" SortKey="code" Property="p => p.Code" Width="8rem" />
        <ThDataTableColumn TItem="ProductDto" Title="Ad" SortKey="name" Property="p => p.Name" />
        <ThDataTableColumn TItem="ProductDto" Title="Fiyat" SortKey="price">
            @context.Price.ToString("C")
        </ThDataTableColumn>
        <ThDataTableColumn TItem="ProductDto" Title="Aktif" Width="6rem">
            @if (context.IsActive)
            {
                <span class="th-badge th-badge--success">Aktif</span>
            }
            else
            {
                <span class="th-badge th-badge--neutral">Pasif</span>
            }
        </ThDataTableColumn>
    </Columns>
</ThDataTable>
```

### Yuva (manuel) modu
Mevcut kullanım korunur:
```razor
<ThDataTable TItem="CustomerDto" Items="_result.Items" Loading="_loading" ColumnCount="6">
    <Header>…</Header>
    <Row Context="c">…</Row>
    <Footer><ThPagination … /></Footer>
</ThDataTable>
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-primary`, `--th-primary-subtle`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-border`, `--th-radius-lg`, `--th-radius-md`, `--th-card-shadow`, `--th-shadow-lg`, `--th-row-pad-y`, `--th-z-dropdown`, `--th-transition-fast`, `--th-ease`, `--th-fw-*`, `--th-font-sans`.

## Tema/uyum notları
- **Tipografi**: Tüm veri satırları, başlık hücreleri ve sayfalama elemanları sistemin `--th-font-sans` değişkenine bağlıdır. Tema tasarımcısından yazı tipi ailesi değiştirildiğinde gerçek zamanlı olarak güncellenir.
- **Renk ve Yoğunluk**: Yoğunluk (`data-density`) modu geliştikçe tablo satır iç dolguları (`--th-row-pad-y`) ile birlikte ölçeklenir. Seçili satır `var(--th-primary-subtle)` zemin alır. Açık + koyu modda doğrulandı.
