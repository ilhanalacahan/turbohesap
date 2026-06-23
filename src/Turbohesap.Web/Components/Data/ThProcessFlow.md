# ThProcessFlow

> İş akışlarını, sipariş adımlarını ve onay durumlarını yatay veya dikey olarak birbirine bağlı düğümlerle görselleştiren süreç izleyici.

- **Sınıf**: `th-process-flow` (+ `--horizontal`, `--vertical`)
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThProcessFlow.razor`, `ThProcessFlow.razor.cs`, `Frontend/css/processflow.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Steps` | `List<ProcessStep>` | `new()` | Süreç adımlarının listesi |
| `Vertical` | `bool` | `false` | True ise dikey akış, false ise yatay akış halinde hizalar |
| `OnStepClick` | `EventCallback<ProcessStep>` | — | Bir adıma tıklandığında fırlatılan olay (delege edilirse adımlar tıklanabilir hover stilini alır) |

### ProcessStep Modeli
- `Id` (`string`): Adım kimliği.
- `Label` (`string`): Adım başlığı.
- `Description` (`string`): Alt detay bilgisi.
- `Icon` (`string`): FontAwesome ikon sınıfı.
- `Status` (`StepStatus`): Adım durumu (`Pending`, `Active`, `Completed`, `Failed`).

## Kullanım
```razor
<ThProcessFlow Steps="@_orderSteps" 
               Vertical="false" 
               OnStepClick="HandleStepClick" />
```

## Kullanılan token'lar
`--th-surface-2`, `--th-surface-3`, `--th-border`, `--th-border-strong`, `--th-text`, `--th-text-subtle`, `--th-text-muted`, `--th-primary`, `--th-primary-subtle`, `--th-success`, `--th-danger`, `--th-radius-full`, `--th-transition-fast`.

## Tema/uyum notları
Aktif adımda (`StepStatus.Active`) otomatik parlayan dalga animasyonu (`th-process-pulse`) gösterir. Koyu ve açık tema renklerine tam uyumludur.
