using System.Globalization;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

/// <summary>Müşteri derecelendirme katmanı (kademe).</summary>
public enum CustomerRatingTier
{
    /// <summary>85+ · Mükemmel / Premium.</summary>
    Excellent,
    /// <summary>65–84 · İyi / Güvenilir.</summary>
    Good,
    /// <summary>45–64 · Orta / İzlenmeli.</summary>
    Average,
    /// <summary>0–44 · Riskli.</summary>
    Risky
}

/// <summary>
/// Müşteri derecelendirme göstergesi. Ödeme zamanlaması, alışveriş hacmi ve sıklığı gibi
/// kriterleri ağırlıklı bir genel puana indirger; bu puanı radyal madalyon, harf notu,
/// yıldız ve kriter dağılım çubuklarıyla görselleştirir. th-customer-rating sarmalayıcısı.
/// </summary>
public partial class ThCustomerRating : TurboComponentBase
{
    /// <summary>Müşteri adı (opsiyonel başlık). Boşsa başlık satırı gizlenir.</summary>
    [Parameter] public string? CustomerName { get; set; }

    /// <summary>İkincil bilgi (ör. cari kod, segment). Opsiyonel.</summary>
    [Parameter] public string? Subtitle { get; set; }

    /// <summary>
    /// Genel puan (0–100). Verilmezse <see cref="Factors"/> üzerinden ağırlıklı ortalama
    /// hesaplanır; o da yoksa 0 kabul edilir.
    /// </summary>
    [Parameter] public double? Score { get; set; }

    /// <summary>Genel puana katkı sağlayan kriterler. Dağılım çubukları bu listeden çizilir.</summary>
    [Parameter] public IReadOnlyList<CustomerRatingFactor>? Factors { get; set; }

    /// <summary>Kriter dağılım çubuklarını gösterir (varsayılan true).</summary>
    [Parameter] public bool ShowBreakdown { get; set; } = true;

    /// <summary>Yıldız satırını gösterir (varsayılan true).</summary>
    [Parameter] public bool ShowStars { get; set; } = true;

    /// <summary>Bileşen boyutu.</summary>
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    /// <summary>0–100 aralığına sıkıştırılmış etkin puan.</summary>
    private double EffectiveScore
    {
        get
        {
            double raw;
            if (Score.HasValue)
            {
                raw = Score.Value;
            }
            else if (Factors is { Count: > 0 })
            {
                var totalWeight = Factors.Sum(f => f.Weight);
                raw = totalWeight > 0
                    ? Factors.Sum(f => f.Value * f.Weight) / totalWeight
                    : Factors.Average(f => f.Value);
            }
            else
            {
                raw = 0;
            }

            return Math.Clamp(raw, 0, 100);
        }
    }

    /// <summary>0–5 arası yıldız değeri (etkin puanın beşte biri).</summary>
    private double StarValue => EffectiveScore / 20d;

    private CustomerRatingTier Tier => TierFor(EffectiveScore);

    private static CustomerRatingTier TierFor(double score) => score switch
    {
        >= 85 => CustomerRatingTier.Excellent,
        >= 65 => CustomerRatingTier.Good,
        >= 45 => CustomerRatingTier.Average,
        _ => CustomerRatingTier.Risky
    };

    private string TierClass => Tier switch
    {
        CustomerRatingTier.Excellent => "th-customer-rating--excellent",
        CustomerRatingTier.Good => "th-customer-rating--good",
        CustomerRatingTier.Average => "th-customer-rating--average",
        _ => "th-customer-rating--risky"
    };

    private string TierLabel => Tier switch
    {
        CustomerRatingTier.Excellent => "Mükemmel",
        CustomerRatingTier.Good => "İyi",
        CustomerRatingTier.Average => "Orta",
        _ => "Riskli"
    };

    private string TierDescription => Tier switch
    {
        CustomerRatingTier.Excellent => "Yüksek hacim, zamanında ödeme — öncelikli müşteri.",
        CustomerRatingTier.Good => "Güvenilir; düzenli alışveriş ve ödeme.",
        CustomerRatingTier.Average => "Ortalama; ödeme veya hacim açısından izlenmeli.",
        _ => "Gecikmeli ödeme / düşük hacim — yakın takip gerekir."
    };

    private string Grade => Tier switch
    {
        CustomerRatingTier.Excellent => "A",
        CustomerRatingTier.Good => "B",
        CustomerRatingTier.Average => "C",
        _ => "D"
    };

    /// <summary>Madalyon konik halkasına geçilen puan yüzdesi (kültürden bağımsız).</summary>
    private string ScoreCss => EffectiveScore.ToString("F1", CultureInfo.InvariantCulture);

    private string ScoreText => Math.Round(EffectiveScore).ToString("N0", CultureInfo.GetCultureInfo("tr-TR"));

    /// <summary>Tek bir yıldızın dolu/yarım/boş durumunu Font Awesome sınıfına çevirir.</summary>
    /// <param name="position">1 tabanlı yıldız sırası (1–5).</param>
    private string StarIconClass(int position)
    {
        if (StarValue >= position) return "fa-solid fa-star";
        if (StarValue >= position - 0.5) return "fa-solid fa-star-half-stroke";
        return "fa-regular fa-star";
    }

    /// <summary>Bir kriterin değerine göre dağılım çubuğu renk sınıfı.</summary>
    private static string FactorClass(double value) => TierFor(value) switch
    {
        CustomerRatingTier.Excellent => "th-customer-rating__factor--excellent",
        CustomerRatingTier.Good => "th-customer-rating__factor--good",
        CustomerRatingTier.Average => "th-customer-rating__factor--average",
        _ => "th-customer-rating__factor--risky"
    };

    private static string FactorWidth(double value)
        => Math.Clamp(value, 0, 100).ToString("F1", CultureInfo.InvariantCulture);

    private string RootClass => Cx(
        "th-customer-rating",
        TierClass,
        SizeClass(Size, "th-customer-rating"),
        Class);
}
