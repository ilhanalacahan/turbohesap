# ThDateRangePicker

> Preset kısayolları ve interaktif çift takvim desteği sunan estetik tarih aralığı seçici bileşeni.

- **Sınıf**: `th-daterange-picker-wrapper`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThDateRangePicker.razor`, `ThDateRangePicker.razor.cs`, `Frontend/css/daterangepicker.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `StartDate` | `DateTime?` | — | Başlangıç tarihi. İki yönlü bağlama destekler. |
| `StartDateChanged` | `EventCallback<DateTime?>` | — | Başlangıç tarihi değiştiğinde tetiklenen olay. |
| `EndDate` | `DateTime?` | — | Bitiş tarihi. İki yönlü bağlama destekler. |
| `EndDateChanged` | `EventCallback<DateTime?>` | — | Bitiş tarihi değiştiğinde tetiklenen olay. |
| `Label` | `string` | `""` | Giriş kutusunun üstündeki başlık metni. |
| `Placeholder` | `string` | `"Tarih Aralığı Seçiniz"` | Boş durumdaki yer tutucu metin. |
| `Disabled` | `bool` | `false` | Seçicinin devre dışı olma durumu. |
| `Size` | `ComponentSize` | `ComponentSize.Md` | Boyut. |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase). |

## Kullanım
```razor
<ThDateRangePicker @bind-StartDate="_baslangicTarihi" 
                   @bind-EndDate="_bitisTarihi" 
                   Label="Filtre Tarih Aralığı" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-surface-3`, `--th-surface-hover`, `--th-border`, `--th-border-strong`, `--th-radius-lg`, `--th-radius-md`, `--th-radius-sm`, `--th-shadow-lg`, `--th-primary`, `--th-primary-subtle`, `--th-primary-fg`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-danger`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Çift takvim seçimi ve preset (bugün, dün, bu ay vb.) seçimleri ile raporlama sayfaları için idealdir.
