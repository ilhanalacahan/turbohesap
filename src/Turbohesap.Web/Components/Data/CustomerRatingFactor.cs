namespace Turbohesap.Web.Components.Data;

/// <summary>
/// Müşteri derecelendirmesine katkı sağlayan tek bir kriter (ödeme zamanlaması,
/// alışveriş hacmi, alışveriş sıklığı...). <see cref="Value"/> 0–100 arası bir
/// "iyilik" puanıdır (yüksek = olumlu). <see cref="Weight"/> kriterin genel
/// puana ağırlığını belirler.
/// </summary>
public sealed class CustomerRatingFactor
{
    /// <summary>Kriter adı (ör. "Ödeme Zamanlaması").</summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>0–100 arası iyilik puanı (yüksek = olumlu).</summary>
    public double Value { get; set; }

    /// <summary>Genel puana ağırlık (varsayılan 1).</summary>
    public double Weight { get; set; } = 1;

    /// <summary>Opsiyonel Font Awesome ikon sınıfı (ör. "fa-solid fa-clock").</summary>
    public string? Icon { get; set; }

    /// <summary>Opsiyonel kısa açıklama / değer etiketi (ör. "Ort. 4 gün gecikme").</summary>
    public string? Description { get; set; }
}
