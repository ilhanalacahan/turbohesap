# ThAutocomplete

> REST veya statik veri destekli, şablonlanabilir ve geciktirmeli otomatik tamamlama (autocomplete) bileşeni.

- **Sınıf**: `th-autocomplete`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThAutocomplete.razor`, `ThAutocomplete.razor.cs`, `Frontend/css/autocomplete.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Items` | `IEnumerable<TItem>?` | — | Statik veri kaynağı |
| `SearchFunc` | `Func<string, Task<IEnumerable<TItem>>>?` | — | REST / API asenkron arama fonksiyonu |
| `ItemTextFunc` | `Func<TItem, string>?` | — | Nesneyi metne dönüştüren fonksiyon |
| `ItemTemplate` | `RenderFragment<TItem>?` | — | Liste elemanı özelleştirme şablonu (template) |
| `MinLength` | `int` | `1` | Aramanın başlaması için minimum girdi uzunluğu |
| `DebounceMs` | `int` | `300` | Girdi sonrası aramanın geciktirilme süresi (milisaniye) |
| `OpenOnFocus` | `bool` | `false` | Odaklanınca otomatik öneri listesinin açılması |
| `Placeholder` | `string?` | — | Giriş yer tutucu metni |
| `Disabled` | `bool` | `false` | Pasiflik durumu |
| `Size` | `ComponentSize` | `Md` | Boyut |
| `OnItemSelected` | `EventCallback<TItem>` | — | Bir öğe seçildiğinde tetiklenecek olay |
| `Value`/`ValueChanged`/`ValueExpression` | — | — | Blazor standart çift yönlü veri bağlama parametreleri |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer |

## Kullanım
```razor
<!-- Statik Liste ile Kullanım -->
<ThAutocomplete TItem="Customer" 
                Items="_customers" 
                ItemTextFunc="c => c.Name" 
                @bind-Value="_selectedCustomer" 
                Placeholder="Müşteri arayın..." />

<!-- Asenkron (API/REST) Arama ve Şablonlu Kullanım -->
<ThAutocomplete TItem="UserDto" 
                SearchFunc="SearchUsersAsync" 
                ItemTextFunc="u => u.Username" 
                @bind-Value="_selectedUser" 
                MinLength="2" 
                DebounceMs="500" 
                OpenOnFocus="true"
                Placeholder="Kullanıcı adı yazın...">
    <ItemTemplate Context="user">
        <div class="flex items-center gap-2 py-1">
            <ThAvatar Name="@user.FullName" Size="ComponentSize.Sm" />
            <div class="flex flex-col">
                <span class="font-medium text-xs">@user.FullName</span>
                <span class="text-text-subtle text-[10px]">@user.Email</span>
            </div>
        </div>
    </ItemTemplate>
</ThAutocomplete>
```

## Kullanılan token'lar
`--th-surface`, `--th-border`, `--th-radius-md`, `--th-radius-sm`, `--th-shadow-lg`, `--th-z-dropdown`, `--th-text`, `--th-surface-3`, `--th-transition-fast`, `--th-ease`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/z-index token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve farklı yoğunluklarda test edilip doğrulanmıştır.
