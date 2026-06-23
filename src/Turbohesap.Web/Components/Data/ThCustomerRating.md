# ThCustomerRating

> Müşteri kalitesini/risk durumunu çoklu kritere göre tek bir görsel notla özetleyen derecelendirme göstergesi.

- **Sınıf**: `th-customer-rating` (+ `--excellent|good|average|risky`, `--sm|md|lg`)
- **Namespace**: `Turbohesap.Web.Components.Data`
- **Dosyalar**: `ThCustomerRating.razor`, `ThCustomerRating.razor.cs`, `CustomerRatingFactor.cs`, `Frontend/css/customerrating.css`

## Amaç

Ödeme zamanlaması, alışveriş hacmi, alışveriş sıklığı, bakiye/risk gibi birden çok kriteri
**ağırlıklı ortalama** ile 0–100 arası tek bir genel puana indirger ve bu puanı görselleştirir:

- **Radyal madalyon**: konik halka puanı dolduğu oranda gösterir; ortada **harf notu** (A–D) ve sayısal puan.
- **Kademe** (tier): puan `>=85` **Mükemmel** (yeşil), `>=65` **İyi** (mavi), `>=45` **Orta** (amber), `<45` **Riskli** (kırmızı).
- **Yıldız satırı**: puanın beşte biri kadar (yarım yıldız destekli) 0–5 yıldız.
- **Kriter dağılımı**: her kriter kendi değer kademesine göre renkli bir çubukla listelenir (geciken ödeme kırmızı, yüksek hacim yeşil…).

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `CustomerName` | `string?` | — | Başlık (müşteri adı). Boşsa başlık satırı gizlenir, kademe rozeti özete taşınır |
| `Subtitle` | `string?` | — | İkincil bilgi (cari kod, segment) |
| `Score` | `double?` | — | Genel puan (0–100). Verilmezse `Factors`'tan ağırlıklı ortalama hesaplanır |
| `Factors` | `IReadOnlyList<CustomerRatingFactor>?` | — | Kriter listesi; dağılım çubukları ve puan hesabı bundan üretilir |
| `ShowBreakdown` | `bool` | `true` | Kriter dağılım çubuklarını gösterir |
| `ShowStars` | `bool` | `true` | Yıldız satırını gösterir |
| `Size` | `ComponentSize` | `Md` | Boyut (madalyon/yazı ölçeği) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke geçer (TurboComponentBase) |

### `CustomerRatingFactor`
| Alan | Tip | Varsayılan | Açıklama |
|------|-----|------------|----------|
| `Label` | `string` | `""` | Kriter adı |
| `Value` | `double` | `0` | 0–100 iyilik puanı (yüksek = olumlu) |
| `Weight` | `double` | `1` | Genel puana ağırlık |
| `Icon` | `string?` | — | Font Awesome ikon sınıfı |
| `Description` | `string?` | — | Kısa değer etiketi (ör. "Ort. 4 gün gecikme") |

## Kullanım
```razor
@code {
    private List<CustomerRatingFactor> _factors = new()
    {
        new() { Label = "Ödeme Zamanlaması", Value = 96, Weight = 2, Icon = "fa-solid fa-clock", Description = "Ort. 1 gün erken" },
        new() { Label = "Alışveriş Hacmi", Value = 92, Weight = 1.5, Icon = "fa-solid fa-sack-dollar", Description = "₺412B / yıl" },
        new() { Label = "Alışveriş Sıklığı", Value = 88, Icon = "fa-solid fa-repeat", Description = "Haftada 3" },
        new() { Label = "Risk / Bakiye", Value = 90, Weight = 1.5, Icon = "fa-solid fa-shield-halved", Description = "Limit içinde" }
    };
}

<ThCustomerRating CustomerName="Mega Yapı Market A.Ş."
                  Subtitle="CARİ-1042 · Bayi"
                  Factors="_factors" />

@* Sadece puanla, dağılımsız ve kompakt *@
<ThCustomerRating Score="58" ShowBreakdown="false" Size="ComponentSize.Sm" />
```

## Kullanılan token'lar
`--th-surface`, `--th-surface-3`, `--th-text`, `--th-text-muted`, `--th-text-subtle`, `--th-border`,
`--th-success(-subtle)`, `--th-primary(-subtle)`, `--th-warning(-subtle)`, `--th-danger(-subtle)`,
`--th-radius-lg`, `--th-radius-full`, `--th-card-shadow`, `--th-content-padding`, `--th-density-gap`,
`--th-fw-medium/semibold/bold`, `--th-transition(-slow)`, `--th-ease`.

## Tema/uyum notları

Tüm renk/yarıçap/gölge/yoğunluk/font değerleri `--th-*` token'larından gelir; kademe renkleri yalnızca
semantik tokenları (`success/primary/warning/danger`) kullandığından açık ve koyu temaya otomatik uyar.
Madalyonun dolma animasyonu `@property --th-cr-score` ile token tabanlı geçiş süresinde yapılır.
Açık + koyu modda doğrulandı.
