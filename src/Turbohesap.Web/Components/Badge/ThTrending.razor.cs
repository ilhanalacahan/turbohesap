using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Badge;

/// <summary>
/// Finansal veriler, satış oranları ve ciro trendleri için yön ikonu barındıran trend rozeti.
/// </summary>
public partial class ThTrending : TurboComponentBase
{
    [Parameter] public double Value { get; set; }
    [Parameter] public bool ShowPercentSign { get; set; } = true;
    [Parameter] public string Format { get; set; } = "F1";
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Sm;

    private string TrendClass => Value > 0 ? "th-trending--up"
                               : Value < 0 ? "th-trending--down"
                               : "th-trending--flat";

    private string IconClass => Value > 0 ? "fa-solid fa-arrow-trend-up"
                              : Value < 0 ? "fa-solid fa-arrow-trend-down"
                              : "fa-solid fa-minus";

    private string FormattedValue => Math.Abs(Value).ToString(Format, CultureInfo.CurrentCulture);

    private string RootClass => Cx(
        "th-trending",
        TrendClass,
        SizeClass(Size, "th-trending"),
        Class);
}
