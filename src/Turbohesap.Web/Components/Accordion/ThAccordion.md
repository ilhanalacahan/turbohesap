# ThAccordion / ThAccordionItem

> Açılır/kapanır içerik grupları. Tek/çoklu açılım, üç varyant ve üç boyut.

- **Sınıf**: `th-accordion` (+ `--bordered|separated|flush`, `--sm|md|lg`); öğe `th-accordion__item` (+ `--open`, `--disabled`)
- **Namespace**: `Turbohesap.Web.Components.Accordion`
- **Dosyalar**: `ThAccordion.razor`, `ThAccordion.razor.cs`, `ThAccordionItem.razor`, `ThAccordionItem.razor.cs`, `AccordionVariant.cs`, `Frontend/css/accordion.css`

`ThAccordion` (kapsayıcı) alt `ThAccordionItem` öğelerinin açık/kapalı durumunu yönetir;
öğeler kendilerini `CascadingValue` ile kayıt eder (`ThTabs`/`ThTabPanel` deseni). Panel,
pürüzsüz yükseklik animasyonu için saf CSS `grid-template-rows: 0fr → 1fr` tekniğini kullanır
(JS yok).

## ThAccordion parametreleri
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | `ThAccordionItem` öğeleri |
| `Variant` | `AccordionVariant` | `Bordered` | `Bordered` / `Separated` / `Flush` |
| `Size` | `ComponentSize` | `Md` | Boyut (`Sm`/`Md`/`Lg`) |
| `Multiple` | `bool` | `false` | Birden çok öğenin aynı anda açık kalması |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

## ThAccordionItem parametreleri
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Header` | `string` | `""` | Başlık metni |
| `Subtitle` | `string?` | — | Başlık altı açıklama |
| `Icon` | `string?` | — | Font Awesome ikon sınıfı |
| `Disabled` | `bool` | `false` | Açılamaz öğe |
| `Open` | `bool` | `false` | Başlangıçta açık |
| `ChildContent` | `RenderFragment?` | — | Açılan panel içeriği |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Öğe köküne geçer |

## Kullanım
```razor
<ThAccordion Variant="AccordionVariant.Separated" Multiple="true">
    <ThAccordionItem Header="Genel Bilgiler" Icon="fa-solid fa-circle-info" Open="true">
        Panel içeriği…
    </ThAccordionItem>
    <ThAccordionItem Header="Adres" Subtitle="Fatura ve teslimat">
        Adres detayları…
    </ThAccordionItem>
    <ThAccordionItem Header="Kilitli Bölüm" Icon="fa-solid fa-lock" Disabled="true">
        Görüntülenemez.
    </ThAccordionItem>
</ThAccordion>
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-text`, `--th-text-muted`, `--th-text-subtle`,
`--th-border`, `--th-primary`, `--th-ring`, `--th-radius-lg`, `--th-card-shadow`,
`--th-nav-pad-y`, `--th-density-gap`, `--th-content-padding`,
`--th-control-height`(`-sm`/`-lg`), `--th-transition`(`-fast`), `--th-ease`, `--th-fw-*`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine
otomatik uyar. Chevron açılınca 90° döner (DESIGN.md §6). Açık + koyu modda doğrulandı.
