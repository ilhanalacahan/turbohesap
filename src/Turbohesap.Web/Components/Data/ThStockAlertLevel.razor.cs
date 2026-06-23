using Microsoft.AspNetCore.Components;
using System;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

/// <summary>
/// Stok seviyesini min ve max limitlerle karşılaştıran görsel indikatör.
/// </summary>
public partial class ThStockAlertLevel : TurboComponentBase
{
    [Parameter] public double Value { get; set; }
    [Parameter] public double MinLimit { get; set; }
    [Parameter] public double MaxLimit { get; set; }
    [Parameter] public string Unit { get; set; } = "Adet";
    [Parameter] public string Label { get; set; } = "Stok Seviyesi";
    [Parameter] public bool ShowLabels { get; set; } = true;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private double TotalScale => MaxLimit > 0 ? MaxLimit * 1.3 : 100;
    
    private double MinPercent => Math.Min(100, Math.Max(0, (MinLimit / TotalScale) * 100));
    private double MaxPercent => Math.Min(100, Math.Max(0, (MaxLimit / TotalScale) * 100));
    private double ValuePercent => Math.Min(100, Math.Max(0, (Value / TotalScale) * 100));

    private string AlertStatusClass => Value < MinLimit ? "th-stock-alert--critical" 
                                     : Value > MaxLimit ? "th-stock-alert--excess" 
                                     : "th-stock-alert--normal";

    private string AlertStatusLabel => Value < MinLimit ? "Kritik" 
                                     : Value > MaxLimit ? "Fazla Stok" 
                                     : "Normal";

    private string RootClass => Cx(
        "th-stock-alert-level",
        AlertStatusClass,
        SizeClass(Size, "th-stock-alert-level"),
        Class);
}
