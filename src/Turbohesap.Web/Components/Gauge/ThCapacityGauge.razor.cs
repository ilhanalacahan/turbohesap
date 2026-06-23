using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Gauge;

/// <summary>
/// SVG tabanlı, dairesel kapasite/doluluk oranını gösteren estetik bileşen.
/// </summary>
public partial class ThCapacityGauge : TurboComponentBase
{
    [Parameter] public double Value { get; set; }
    [Parameter] public double Max { get; set; } = 100;
    [Parameter] public string Unit { get; set; } = "%";
    [Parameter] public string Label { get; set; } = "Kapasite";
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public bool ShowDetails { get; set; } = true;
    [Parameter] public double ThresholdWarning { get; set; } = 75; // %75 üstü uyarı
    [Parameter] public double ThresholdDanger { get; set; } = 90;  // %90 üstü tehlike
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    // Sabitler (SVG çember hesaplaması için)
    protected const double Radius = 40;
    protected const double Circumference = 2 * Math.PI * Radius; // ~251.327
    protected const double ArcLength = Circumference * 0.75;      // 270 derecelik yay (~188.5)

    protected double Percent
    {
        get
        {
            if (Max <= 0) return 0;
            var pct = (Value / Max) * 100;
            return Math.Min(100, Math.Max(0, pct));
        }
    }

    protected double FilledLength => (Percent / 100.0) * ArcLength;

    protected string StatusClass => Percent >= ThresholdDanger ? "th-capacity-gauge--danger"
                                  : Percent >= ThresholdWarning ? "th-capacity-gauge--warning"
                                  : "th-capacity-gauge--success";

    protected string RootClass => Cx(
        "th-capacity-gauge",
        StatusClass,
        SizeClass(Size, "th-capacity-gauge"),
        Class);
}
