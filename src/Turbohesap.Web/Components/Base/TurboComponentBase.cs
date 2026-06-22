using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Components.Base;

/// <summary>Bileşen boyutu. Açık sınıf adlarına eşlenir (req 20).</summary>
public enum ComponentSize { Sm, Md, Lg }

/// <summary>Bileşen köşe yarıçapı.</summary>
public enum ComponentRadius { None, Sm, Md, Lg, Full }

/// <summary>
/// Tüm Turbohesap bileşenlerinin temeli (req 19). Ortak <c>Class</c>, <c>Style</c> ve
/// yakalanmamış öznitelikleri taşır. Boyut/yarıçap gibi alanlar, ihtiyaç duyan
/// bileşenlerde eklenir ve daima sabit sınıf adlarına eşlenir (req 20).
/// </summary>
public abstract class TurboComponentBase : ComponentBase
{
    /// <summary>Bileşenin köküne eklenecek ek sınıflar.</summary>
    [Parameter] public string? Class { get; set; }

    /// <summary>Bileşenin köküne eklenecek satır içi stil.</summary>
    [Parameter] public string? Style { get; set; }

    /// <summary>Yakalanmamış HTML öznitelikleri (data-*, aria-*, id...).</summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>Boş/null sınıfları atlayarak birleştirir.</summary>
    protected static string Cx(params string?[] classes)
        => string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));

    /// <summary>Boyutu sabit bir CSS son ekine çevirir (req 20: string interpolasyonu yok).</summary>
    protected static string SizeClass(ComponentSize size, string prefix) => size switch
    {
        ComponentSize.Sm => $"{prefix}--sm",
        ComponentSize.Lg => $"{prefix}--lg",
        _ => $"{prefix}--md"
    };
}
