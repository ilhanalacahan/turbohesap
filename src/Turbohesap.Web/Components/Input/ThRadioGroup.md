# ThRadioGroup & ThRadio

> Yatay ve dikey yerleşim desteği olan, estetik radyo seçenek grubu.

- **Sınıf**: `th-radio-group` (+ `--vertical`, `--horizontal`), `th-radio` (+ `--disabled`, `--checked`)
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThRadioGroup.razor`, `ThRadioGroup.razor.cs`, `ThRadio.razor`, `ThRadio.razor.cs`, `Frontend/css/input.css`

## Parametreler (ThRadioGroup)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ChildContent` | `RenderFragment?` | — | Radyo buton seçenekleri (`ThRadio` listesi) |
| `Layout` | `RadioLayout` | `Horizontal` | Seçeneklerin dizilim yönü (`Horizontal` / `Vertical`) |
| `Disabled` | `bool` | `false` | Tüm grubun pasiflik durumu |
| `Value`/`ValueChanged`/`ValueExpression` | — | — | Blazor standart çift yönlü veri bağlama parametreleri |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer |

## Parametreler (ThRadio)
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `TValue?` | — | Seçeneğin değeri |
| `Label` | `string?` | — | Seçenek metni |
| `ChildContent` | `RenderFragment?` | — | Özelleştirilmiş seçenek içeriği |
| `Disabled` | `bool` | `false` | Seçeneğin pasiflik durumu |
| `Class`/`Style` | — | — | Köke geçer |

## Kullanım
```razor
<ThRadioGroup @bind-Value="_selectedOption" Layout="RadioLayout.Vertical">
    <ThRadio Value="1" Label="Seçenek 1" />
    <ThRadio Value="2" Label="Seçenek 2" />
    <ThRadio Value="3" Label="Seçenek 3 (Pasif)" Disabled="true" />
</ThRadioGroup>
```

## Kullanılan token'lar
`--th-surface`, `--th-text`, `--th-border-strong`, `--th-radius-full`, `--th-primary`, `--th-ring`, `--th-surface-3`, `--th-border`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve farklı yoğunluklarda test edilip doğrulanmıştır.
