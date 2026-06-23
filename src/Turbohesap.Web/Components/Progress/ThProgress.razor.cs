using Microsoft.AspNetCore.Components;
using System;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Progress;

public enum ProgressVariant { Primary, Success, Warning, Danger, Info }

/// <summary>
/// Erişilebilir ve özelleştirilmiş progress bar (th-progress) bileşeni.
/// </summary>
public partial class ThProgress : TurboComponentBase
{
    [Parameter] public double Value { get; set; }
    [Parameter] public double Max { get; set; } = 100;
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool ShowValue { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public ProgressVariant Variant { get; set; } = ProgressVariant.Primary;

    private double Percentage => Math.Max(0, Math.Min(100, (Value / Max) * 100));
    private string PercentageCss => $"{Percentage.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture)}%";

    private string RootClass => Cx(
        "th-progress-wrap",
        SizeClass(Size, "th-progress"),
        Variant switch
        {
            ProgressVariant.Success => "th-progress--success",
            ProgressVariant.Warning => "th-progress--warning",
            ProgressVariant.Danger => "th-progress--danger",
            ProgressVariant.Info => "th-progress--info",
            _ => null
        },
        Class);
}
