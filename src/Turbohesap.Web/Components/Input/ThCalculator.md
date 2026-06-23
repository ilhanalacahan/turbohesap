# ThCalculator

> Ticari ve finansal işlemler için KDV butonlu, modern ve estetik hesap makinesi bileşeni.

- **Sınıf**: `th-calculator`
- **Namespace**: `Turbohesap.Web.Components.Input`
- **Dosyalar**: `ThCalculator.razor`, `ThCalculator.razor.cs`, `Frontend/css/calculator.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `InitialValue` | `decimal` | `0` | Hesap makinesi açıldığında ekranda gösterilecek başlangıç değeri. |
| `OnValueSelected` | `EventCallback<decimal>` | — | Hesaplama onaylandığında (Ctrl+Enter veya onay butonuyla) nihai sonucu döndüren olay. |
| `OnCancel` | `EventCallback` | — | İşlem iptal edildiğinde (ESC tuşu veya iptalle) tetiklenen olay. |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase). |

## Klavye Kısayolları ve Davranışlar
*   **Sayılar (0-9) & Operatörler (+, -, *, /)**: Doğrudan tuş takımı gibi çalışır.
*   **Virgül (,) / Nokta (.)**: Ondalık ayraç ekler.
*   **Backspace**: Son karakteri siler.
*   **Escape (ESC)**: İşlemi iptal edip kapatır.
*   **C / c / Delete (Del)**: Ekranı temizler.
*   **Enter / =**: Formülü hesaplayıp sadece ekrana yazar. Değeri geri aktarıp kapatmaz.
*   **Ctrl + Enter**: Formülü hesaplar, nihai değeri ana inputa aktarır ve diyalogu kapatır.

## Kullanım
```razor
<ThCalculator InitialValue="150.50M" 
              OnValueSelected="OnCalculated" 
              OnCancel="OnCancel" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-2`, `--th-surface-3`, `--th-surface-hover`, `--th-border`, `--th-border-hover`, `--th-radius-lg`, `--th-radius-md`, `--th-card-shadow`, `--th-primary`, `--th-primary-subtle`, `--th-danger`, `--th-danger-subtle`, `--th-success`, `--th-success-subtle`, `--th-font-mono`.

## Tema/uyum notları
Renk/yarıçap/gölge/yoğunluk/font token'larına bağlıdır; tema tasarımcısı değişikliklerine otomatik uyar. Açık + koyu modda ve klavye kısayolu ile hızlı kullanımda test edilmiştir.
