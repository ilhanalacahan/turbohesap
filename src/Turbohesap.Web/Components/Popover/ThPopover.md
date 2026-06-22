# ThPopover

> Tıklamayla açılan, içerisinde buton veya form elemanları yerleştirilebilen gelişmiş balon pencere bileşeni.

- **Sınıf**: `th-popover-wrap`, `th-popover-backdrop`, `th-popover-content` (+ `--top|bottom|left|right`), `th-popover-header`, `th-popover-body`, `th-popover-close`
- **Namespace**: `Turbohesap.Web.Components.Popover`
- **Dosyalar**: `ThPopover.razor`, `ThPopover.razor.cs`, `Frontend/css/popover.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Trigger` | `RenderFragment?` | — | Tıklama tetikleyicisi olan element yuvası |
| `ChildContent` | `RenderFragment?` | — | Balon pencere gövdesi içeriği yuvası |
| `Title` | `string?` | — | İsteğe bağlı üst başlık metni |
| `ShowCloseButton` | `bool` | `true` | Başlıkta kapatma çarpı butonunu gösterir |
| `Position` | `PopoverPosition` | `Bottom` | Açılış konumu (`Bottom`, `Top`, `Left`, `Right`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThPopover Title="Hızlı Filtrele" Position="PopoverPosition.Bottom">
    <Trigger>
        <ThButton Variant="ButtonVariant.Secondary" Icon="fa-solid fa-filter">Filtre</ThButton>
    </Trigger>
    <ChildContent>
        <div class="flex flex-col gap-2">
            <ThCheckbox Label="Sadece Aktifler" />
            <ThButton Size="ComponentSize.Sm">Uygula</ThButton>
        </div>
    </ChildContent>
</ThPopover>
```

## Kullanılan token'lar
- `--th-surface`
- `--th-surface-2`
- `--th-border`
- `--th-radius-md`
- `--th-shadow-lg`
- `--th-text`
- `--th-text-muted`
- `--th-transition-fast`
- `--th-ease`
- `--th-z-dropdown`

## Tema/uyum notları
- **Dışarı Tıklama (Click-Away)**: Popover açıldığında görünmez, sabit bir arka plan perdesi (`th-popover-backdrop`) oluşturulur; popover dışına tıklandığında otomatik olarak kapanma olayı tetiklenir.
- **Koyu Tema**: Zemin, kenarlık ve gölge parametreleri tema değişkenlerine bağlıdır.
