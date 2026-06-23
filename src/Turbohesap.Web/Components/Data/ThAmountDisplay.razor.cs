using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

public enum SymbolPosition
{
    Prepend,
    Append
}

/// <summary>
/// Finansal tutarları kuruş hanesini küçülterek ve işaretine göre (borç/alacak) renklendirerek gösteren estetik bileşen.
/// </summary>
public partial class ThAmountDisplay : TurboComponentBase
{
    private static readonly CultureInfo TrCulture = new CultureInfo("tr-TR");

    [Parameter] public decimal Amount { get; set; }
    [Parameter] public string CurrencySymbol { get; set; } = "₺";
    [Parameter] public SymbolPosition SymbolPosition { get; set; } = SymbolPosition.Append;
    [Parameter] public bool ColorCode { get; set; } = true;
    [Parameter] public CultureInfo Culture { get; set; } = TrCulture;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string IntegerPart { get; set; } = "0";
    private string DecimalPart { get; set; } = "00";
    private string DecimalSeparator { get; set; } = ",";

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        FormatAmount();
    }

    private void FormatAmount()
    {
        // Mutlak değeri formatla
        var absoluteAmount = Math.Abs(Amount);
        var formatted = absoluteAmount.ToString("N2", Culture);

        // Kültüre göre ondalık ayracını bul (tr-TR için "," , en-US için ".")
        DecimalSeparator = Culture.NumberFormat.NumberDecimalSeparator;

        var sepIndex = formatted.LastIndexOf(DecimalSeparator, StringComparison.Ordinal);
        if (sepIndex >= 0)
        {
            IntegerPart = formatted[..sepIndex];
            DecimalPart = formatted[(sepIndex + 1)..];
        }
        else
        {
            IntegerPart = formatted;
            DecimalPart = "00";
        }
    }

    private string SignPrefix => Amount < 0 ? "-" : "";

    private string StatusColorClass => !ColorCode ? "th-amount-display--neutral"
                                     : Amount > 0 ? "th-amount-display--positive"
                                     : Amount < 0 ? "th-amount-display--negative"
                                     : "th-amount-display--neutral";

    private string RootClass => Cx(
        "th-amount-display",
        StatusColorClass,
        SizeClass(Size, "th-amount-display"),
        Class);
}
