# ThBreadcrumb

> Sayfa hiyerarşisini gösteren ve dinamik navigasyon ağacına bağlı çalışan ekmek kırıntısı patikası.

- **Sınıf**: `th-breadcrumb`, `th-breadcrumb__item`, `th-breadcrumb__link`, `th-breadcrumb__item--active`, `th-breadcrumb__separator`
- **Namespace**: `Turbohesap.Web.Components.Breadcrumb`
- **Dosyalar**: `ThBreadcrumb.razor`, `ThBreadcrumb.razor.cs`, `BreadcrumbItem.cs`, `Frontend/css/breadcrumb.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `Items` | `List<BreadcrumbItem>` | `[]` | Patika öğelerinin listesi |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## BreadcrumbItem alanları
| Alan | Tip | Açıklama |
|------|-----|----------|
| `Text` | `string` | Gösterilecek metin |
| `Url` | `string?` | Tıklanınca gidilecek adres |
| `Icon` | `string?` | İsteğe bağlı sol ikon sınıfı (FontAwesome) |

## Kullanım
```razor
<ThBreadcrumb Items="_breadcrumbItems" />
```

## Kullanılan token'lar
- `--th-text`
- `--th-text-muted`
- `--th-text-subtle`
- `--th-primary`
- `--th-transition-fast`
- `--th-ease`
- `--th-font-sans`

## Tema/uyum notları
- **Erişilebilirlik**: `aria-label="Ekmek Kırıntısı"` ile sarmalanır; son öğe `aria-current="page"` semantiğini yansıtacak şekilde tıklanamaz ve vurgulu durum alır.
- **Koyu Tema**: Bağlantı hover renkleri ve pasif ayraç tonları tema parametreleriyle dinamik geçiş yapar.
