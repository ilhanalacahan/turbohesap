# ThListView

> Statik veya asenkron veri destekli, seçilebilir, aranabilir ve sonsuz kaydırmalı (infinite scroll) liste görünümü.

- **Sınıf**: `th-listview`
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThListView.razor`, `ThListView.razor.cs`, `Frontend/css/listview.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Items` | `IEnumerable<TItem>?` | — | Statik veri kaynağı |
| `SearchFunc` | `Func<string, Task<IEnumerable<TItem>>>?` | — | REST / API asenkron arama fonksiyonu |
| `ItemTemplate` | `RenderFragment<TItem>?` | — | Liste satır elemanı özelleştirme şablonu (template) |
| `ItemTitle` | `Func<TItem, string>?` | — | Standart yerleşim için başlık seçici fonksiyonu |
| `ItemSubtitle` | `Func<TItem, string>?` | — | Standart yerleşim için alt başlık seçici fonksiyonu |
| `ItemLeftImage` | `Func<TItem, string>?` | — | Standart yerleşim için sol resim URL seçici fonksiyonu |
| `ItemLeftIcon` | `Func<TItem, string>?` | — | Standart yerleşim için sol ikon sınıfı seçici fonksiyonu |
| `ItemRightText` | `Func<TItem, string>?` | — | Standart yerleşim için sağ metin seçici fonksiyonu |
| `ItemRightBadge` | `Func<TItem, string>?` | — | Standart yerleşim için sağ rozet metni seçici fonksiyonu |
| `ItemRightBadgeVariant`| `Func<TItem, string>?` | — | Standart yerleşim rozet varyantı (`primary`, `success` vb.) |
| `Searchable` | `bool` | `false` | Arama çubuğunun gösterilip gösterilmeyeceği |
| `SearchPlaceholder` | `string` | `"Ara..."` | Arama çubuğu yer tutucu metni |
| `SelectionMode` | `ListViewSelectionMode`| `None` | Seçim modu (`None` / `Single` / `Multiple`) |
| `ShowCheckbox` | `bool` | `false` | Çoklu seçim modunda seçim kutularının gösterilmesi |
| `InfiniteScroll` | `bool` | `false` | Sonsuz kaydırma (scroll ile yükleme) aktifliği |
| `HasMore` | `bool` | `false` | Sonsuz kaydırmada daha fazla veri olup olmadığı bilgisi |
| `OnLoadMore` | `EventCallback` | — | Sonsuz kaydırmada en alta gelince tetiklenecek olay |
| `Height` | `string` | `"400px"` | Liste maksimum yüksekliği (scroll çıkması için) |
| `SelectedItem` | `TItem?` | — | Tekli seçimde seçili eleman (iki yönlü bind edilebilir) |
| `SelectedItems` | `List<TItem>` | — | Çoklu seçimde seçili elemanlar listesi (iki yönlü bind edilebilir) |
| `Class`/`Style`/`AdditionalAttributes`| — | — | Köke geçer |

## Kullanım
```razor
<!-- Standart Şablon & Arama & Tekli Seçim -->
<ThListView TItem="Customer" 
            Items="_customers" 
            ItemTitle="c => c.Name" 
            ItemSubtitle="c => c.Email" 
            ItemLeftIcon='c => "fa-solid fa-user"' 
            Searchable="true" 
            SelectionMode="ListViewSelectionMode.Single" 
            @bind-SelectedItem="_selectedCustomer" />

<!-- Özel Şablon & Çoklu Seçim & Checkbox -->
<ThListView TItem="Product" 
            Items="_products" 
            SelectionMode="ListViewSelectionMode.Multiple" 
            ShowCheckbox="true"
            @bind-SelectedItems="_selectedProducts">
    <ItemTemplate Context="product">
        <div class="flex items-center justify-between w-full py-1">
            <span class="font-medium text-xs">@product.Name</span>
            <span class="text-xs text-primary font-semibold">@product.Price TL</span>
        </div>
    </ItemTemplate>
</ThListView>
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-surface-3`, `--th-border`, `--th-radius-lg`, `--th-radius-md`, `--th-card-shadow`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-primary`, `--th-primary-subtle`, `--th-transition-fast`, `--th-ease`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve farklı yoğunluklarda test edilip doğrulanmıştır.
