# ThMaskInput

> Telefon, TC Kimlik No, IBAN, plaka veya özel kalıplar için dinamik maskeleme desteği sunan giriş kutusu.

- **Sınıf**: `th-mask-input-wrapper`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThMaskInput.razor`, `ThMaskInput.razor.cs`, `Frontend/css/maskinput.css`, JS Interop (`initMaskInput`, `setMaskInputValue`, `disposeMaskInput`)

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Value` | `string?` | — | Giriş kutusuna bağlı iki yönlü değer. |
| `ValueChanged` | `EventCallback<string?>` | — | Değer değiştiğinde tetiklenen olay. |
| `Label` | `string` | `""` | Giriş kutusunun üstündeki başlık metni. |
| `Placeholder` | `string` | `""` | Boş durumdaki yer tutucu metin. |
| `Mask` | `string` | `"0(###) ### ## ##"` | Maske formatı (`#` rakam, `?` harf, `*` alfanümerik). |
| `ReturnRawValue` | `bool` | `true` | `true` ise ham girdileri (örn. "3465121017"), `false` ise formatlı halini (örn. "0(346) 512 10 17") döndürür. |
| `Disabled` | `bool` | `false` | Giriş kutusunun devre dışı olma durumu. |
| `Size` | `ComponentSize` | `ComponentSize.Md` | Bileşen boyutu. |
| `Prepend` | `RenderFragment?` | — | Sol tarafa eklenecek buton veya sabit etiket slotu. |
| `Append` | `RenderFragment?` | — | Sağ tarafa eklenecek buton veya sabit etiket slotu. |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase). |

## Gelişmiş Davranışlar ve Mimariler
*   **Alfanümerik ve Harf Maskelemesi**: Maskedeki `#` karakteri sadece rakamları (`0-9`), `?` karakteri ise sadece harfleri (`A-Za-z`) kabul eder ve harfleri otomatik büyük harfe dönüştürür. `*` ise hem harf hem rakam kabul eder.
*   **Özel Dinamik Plaka Modu (`Mask="PLAKA"`)**: Maske parametresine `"PLAKA"` (veya `"plaka"`) verildiğinde, Türkiye plaka standartlarına uygun olarak 1, 2 veya 3 harfli; ve 2, 3 veya 4 rakamlı tüm plakaları (örn: `34 ABC 123`, `58 AH 312`, `06 T 1234`) dinamik olarak biçimlendirir ve boşlukları otomatik yerleştirir.
*   **Akıllı Önek/Sonek Karakter Tüketimi**: Maske kalıbının başında veya ortasında sabit bir rakam yer aldığında (örn. `0` veya `TR` ile başlayan maskeler), kullanıcının girdiği ilk rakamın bu sabit karakterle çakışması durumunda mükerrer karakter eklemelerini önlemek için akıllı karakter tüketim mantığı uygulanır.
*   **Blazor DOM Senkronizasyonu (`setMaskInputValue`)**: Yazma anında Blazor'ın DOM'u ezip imleç (cursor) konumunu kaydırmasını önlemek için değerler tamamen JS-Interop üzerinden senkronize edilir.

## Kullanım Örnekleri
### Telefon Numarası Maskesi (Ham Değer Dönen)
```razor
<ThMaskInput Value="_telNo" 
             ValueChanged="val => _telNo = val"
             Label="Telefon Numarası" 
             Mask="0(###) ### ## ##" 
             ReturnRawValue="true" 
             Placeholder="0(555) 555 55 55" />
```

### Sol Butonlu IBAN Numarası Maskesi (TR Butonlu)
```razor
<ThMaskInput Value="_ibanNo" 
             ValueChanged="val => _ibanNo = val"
             Label="IBAN Numarası" 
             Mask="#### #### #### #### #### #### ##" 
             ReturnRawValue="true" 
             Placeholder="2600 0100 0000 0000 0000 0000 00">
    <Prepend>
        <ThButton Variant="ButtonVariant.Subtle" OnClick="CopyIban">TR</ThButton>
    </Prepend>
</ThMaskInput>
```

### Plaka Girişi (Dinamik Formatlı - PLAKA)
```razor
<ThMaskInput @bind-Value="_plate" 
             Label="Plaka Girişi" 
             Mask="PLAKA" 
             ReturnRawValue="false" 
             Placeholder="34 ABC 123 veya 58 AH 312" />
```

### Sağ Sonek Butonlu Giriş (Dosya Adı)
```razor
<ThMaskInput @bind-Value="_fileName" 
             Label="Dosya Adı" 
             Mask="********************" 
             ReturnRawValue="true" 
             Placeholder="dosya_adi">
    <Append>
        <ThButton Variant="ButtonVariant.Subtle" OnClick="SelectExtension">.jpeg</ThButton>
    </Append>
</ThMaskInput>
```

## Kullanılan token'lar
`--th-border`, `--th-radius-md`, `--th-text-subtle`, `--th-surface-2`.

## Tema/uyum notları
Yazma anındaki pürüzsüz JS interop senkronizasyonuna dayanır. Koyu tema ve farklı ekran genişlikleriyle tam uyumludur. Prepend ve Append alanları `input.css` içerisindeki entegre buton kenarlıklarıyla pürüzsüz bir "button-input" görünümü oluşturur.
