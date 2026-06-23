# ThInput

> İkonlu, butonlu veya standart biçimde kullanılabilen estetik giriş alanları.

- **Sınıf**: `th-input-wrapper-outer` (+ `--disabled`, `--error`, `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThInput.razor`, `ThInput.razor.cs`, `Frontend/css/input.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Type` | `string?` | `"text"` | Giriş tipi (text, password, email, number vb.) |
| `Placeholder` | `string?` | — | Yer tutucu metin |
| `Disabled` | `bool` | `false` | Pasiflik durumu |
| `Size` | `ComponentSize` | `Md` | Boyut |
| `LeadingIcon` | `string?` | — | Sol ikon sınıfı (Font Awesome, ör. `fa-solid fa-envelope`) |
| `TrailingIcon` | `string?` | — | Sağ ikon sınıfı (Font Awesome, ör. `fa-solid fa-lock`) |
| `Prepend` | `RenderFragment?` | — | Sol tarafa eklenecek şablon (örn. Butonlar) |
| `Append` | `RenderFragment?` | — | Sağ tarafa eklenecek şablon (örn. Butonlar) |
| `Autocomplete` | `string?` | — | Otomatik tamamlama özniteliği |
| `Value`/`ValueChanged`/`ValueExpression` | — | — | Blazor standart çift yönlü veri bağlama parametreleri |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase benzeri aktarım) |

## Kullanım
```razor
<!-- Standart ve İkonlu Kullanım -->
<ThInput @bind-Value="_email" Placeholder="E-posta Adresi" LeadingIcon="fa-solid fa-envelope" />

<!-- Butonlu Kullanım -->
<ThInput @bind-Value="_search" Placeholder="Ara..." Size="ComponentSize.Lg">
    <Append>
        <ThButton Variant="ButtonVariant.Primary" OnClick="OnSearch">
            <i class="fa-solid fa-magnifying-glass"></i>
        </ThButton>
    </Append>
</ThInput>
```

## Kullanılan token'lar
`--th-surface`, `--th-text`, `--th-border-strong`, `--th-radius-md`, `--th-primary`, `--th-ring`, `--th-danger`, `--th-control-height`, `--th-control-height-sm`, `--th-control-height-lg`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve farklı yoğunluklarda test edilip doğrulanmıştır.
