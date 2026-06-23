# ThTagsInput

> Kullanıcının etiketleri virgül veya enter tuşuyla eklediği ve backspace ile silebildiği çoklu etiket/kategori giriş kutusu.

- **Sınıf**: `th-tags-input` (+ `--focused`, `--disabled`, `--error`, `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThTagsInput.razor`, `ThTagsInput.razor.cs`, `Frontend/css/tagsinput.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `List<string>?` | — | Bağlanan etiket listesi |
| `ValueChanged` | `EventCallback<List<string>>` | — | Etiket listesi değiştiğinde tetiklenen olay |
| `ValueExpression` | `Expression<Func<List<string>>>?` | — | EditContext entegrasyonu için ifade |
| `Placeholder` | `string?` | `"Etiket ekleyin..."` | Giriş kutusunda gösterilecek ipucu metni |
| `Disabled` | `bool` | `false` | Giriş kutusunun devre dışı olma durumu |
| `Size` | `ComponentSize` | `ComponentSize.Md` | Boyut varyantı (`Sm`, `Md`, `Lg`) |
| `MaxTags` | `int` | `0` | Eklenebilecek maksimum etiket sayısı (0 = Sınırsız) |
| `Class` | `string?` | — | Köke eklenecek ek CSS sınıfı |
| `Style` | `string?` | — | Köke eklenecek ek satır içi stil |

## Kullanım
```razor
<ThTagsInput @bind-Value="_myTags" Placeholder="Kategoriler..." MaxTags="5" />
```

## Kullanılan token'lar
- `--th-control-height`, `--th-control-height-sm`, `--th-control-height-lg` (Yükseklik varyasyonları)
- `--th-surface`, `--th-surface-2`, `--th-surface-3`, `--th-surface-hover` (Arka plan renkleri)
- `--th-border`, `--th-border-strong` (Kenarlık renkleri)
- `--th-text`, `--th-text-subtle` (Yazı renkleri)
- `--th-primary`, `--th-ring` (Odaklanma durumundaki renkler)
- `--th-danger`, `--th-danger-subtle` (Hata durumundaki renkler)
- `--th-radius-md`, `--th-radius-sm`, `--th-radius-full` (Köşe yarıçapları)
- `--th-transition-fast`, `--th-ease` (Animasyon geçişleri)

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve form doğrulamalarında (validation) tam uyumludur.
