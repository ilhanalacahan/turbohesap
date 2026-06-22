# ThTabs & ThTabPanel

> Pozisyonlanabilir, carousel kaydırmalı (sağ/sol yön okları), mouse tekerleği destekli, kapatılabilir, engellenebilir (disabled), lazy loading destekli ve iptal edilebilir kapatma onay olaylı zengin sekme bileşeni.

- **Sınıf**: `th-tabs` (+ varyantlar: `th-tabs--line`, `th-tabs--pills`, `th-tabs--bordered`, `th-tabs--flat`)
- **Namespace**: `Turbohesap.Web.Components.Tabs`
- **Dosyalar**: `ThTabs.razor`, `ThTabs.razor.cs`, `ThTabPanel.razor`, `ThTabPanel.razor.cs`, `TabVariant.cs`, `TabCloseEventArgs.cs`, `Frontend/scripts/tabs.ts`, `Frontend/css/tabs.css`

## Parametreler (ThTabs)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | Sekme panellerinin (`ThTabPanel`) listesi. |
| `Variant` | `TabVariant` | `TabVariant.Line` | Sekme görsel stili (`Line`, `Pills`, `Bordered`, `Flat`). |
| `Lazy` | `bool` | `false` | true ise sekmelerin içeriği yalnızca o sekme aktif olduğunda yüklenir (Lazy Loading). |
| `OnTabClose` | `EventCallback<TabCloseEventArgs>` | — | Herhangi bir sekme kapatılırken tetiklenir (iptal edilebilir). |

## Parametreler (ThTabPanel)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Header` | `string` | — | Sekme başlık metni (zorunlu). |
| `Icon` | `string?` | — | Sekme başlığında gösterilecek Font Awesome ikon sınıfı (örn. `fa-solid fa-home`). |
| `Disabled` | `bool` | `false` | true ise sekme seçilemez. |
| `Closable` | `bool` | `false` | true ise sekme kapatma butonu (`x`) ile kapatılabilir. |
| `OnClose` | `EventCallback<TabCloseEventArgs>` | — | Bu sekme özelinde kapatılırken tetiklenir (iptal edilebilir). |
| `ChildContent` | `RenderFragment?` | — | Sekme içeriği. |

## Varyantlar / Boyutlar
- `.th-tabs--line`: Alttan 2px çizgili klasik sekme (varsayılan).
- `.th-tabs--pills`: Arka planı hap şeklinde dolgulu sekme.
- `.th-tabs--bordered`: Aktif sekmenin etrafında kart çerçevesi oluşturan sekme.
- `.th-tabs--flat`: Minimalist, hover/aktif durumlarında hafif yumuşak zemin rengi alan sekme.

## Kullanım

### 1. Basit Sekmeler (Farklı Varyantlar ve Engelli Durum)
```razor
<ThTabs Variant="TabVariant.Pills">
    <ThTabPanel Header="Profil" Icon="fa-solid fa-user">
        Profil bilgileri...
    </ThTabPanel>
    <ThTabPanel Header="Ayarlar" Icon="fa-solid fa-gear">
        Uygulama ayarları...
    </ThTabPanel>
    <ThTabPanel Header="Gelişmiş" Icon="fa-solid fa-code" Disabled="true">
        Bu alan engellenmiştir.
    </ThTabPanel>
</ThTabs>
```

### 2. Kapatılabilir ve Onay Kutulu (Cancellable Close) Sekmeler
```razor
<ThTabs Lazy="true">
    <ThTabPanel Header="Kapatılabilir Sekme" Closable="true" OnClose="OnTabClosing">
        Kapatılmadan önce onay soracaktır.
    </ThTabPanel>
</ThTabs>

@code {
    @inject ThDialogService Dialog

    private async Task OnTabClosing(TabCloseEventArgs args)
    {
        var confirm = await Dialog.ConfirmAsync("Emin misiniz?", "Sekmeyi kapatmak istiyor musunuz?", DialogVariant.Warning);
        if (confirm.Status != DialogStatus.Confirmed)
        {
            args.Cancel = true; // Kapatmayı iptal et
        }
    }
}
```

## Kullanılan token'lar
- `--th-border`
- `--th-surface`
- `--th-surface-2`
- `--th-surface-3`
- `--th-text`
- `--th-text-muted`
- `--th-text-subtle`
- `--th-primary`
- `--th-primary-fg`
- `--th-primary-subtle`
- `--th-radius-xs`
- `--th-radius-md`
- `--th-radius-lg`
- `--th-transition-fast`
- `--th-ease`

## Tema/uyum notları
- Sekmelerin tüm renkleri, aktif durum belirteçleri ve kenarlıkları seçili temaya (`light` / `dark`) anında uyum sağlar.
- Carousel okları ve sekme sığmama durumundaki kaydırma mekanizmaları mobil ve masaüstü tarayıcılarda performansı optimize etmek için MutationObserver ile izlenir ve tamamen client-side/saf JS olarak yönetilir.
