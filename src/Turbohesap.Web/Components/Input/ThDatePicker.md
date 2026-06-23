# ThDatePicker

> Tarih, saat veya tarih-saat girişi yapabilen, ikon destekli ve ThInput ile görsel olarak uyumlu estetik zaman seçici.

- **Sınıf**: `th-input-wrapper-outer` (+ `--disabled`, `--error`, `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThDatePicker.razor`, `ThDatePicker.razor.cs`, `Frontend/css/input.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Type` | `DatePickerType` | `DatePickerType.Date` | Seçim tipi (`Date` / `Time` / `DateTime`) |
| `Disabled` | `bool` | `false` | Pasiflik durumu |
| `Size` | `ComponentSize` | `Md` | Boyut |
| `LeadingIcon` | `string?` | — | Sol ikon sınıfı (Boşsa tipe göre otomatik atanır) |
| `Value`/`ValueChanged`/`ValueExpression` | — | — | Blazor standart çift yönlü veri bağlama parametreleri |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer |

## Kullanım
```razor
<!-- Tarih Seçici -->
<ThDatePicker TValue="DateTime?" @bind-Value="_birthDate" Type="DatePickerType.Date" />

<!-- Saat Seçici -->
<ThDatePicker TValue="TimeOnly?" @bind-Value="_meetingTime" Type="DatePickerType.Time" />

<!-- Tarih Saat Seçici -->
<ThDatePicker TValue="DateTime" @bind-Value="_eventDateTime" Type="DatePickerType.DateTime" />
```

## Kullanılan token'lar
`--th-surface`, `--th-text`, `--th-border-strong`, `--th-radius-md`, `--th-primary`, `--th-ring`, `--th-danger`, `--th-control-height`, `--th-control-height-sm`, `--th-control-height-lg`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve farklı yoğunluklarda test edilip doğrulanmıştır.
