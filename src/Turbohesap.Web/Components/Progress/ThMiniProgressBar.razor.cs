using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Progress;

public class ProgressSegment
{
    public string Label { get; set; } = "";
    public double Value { get; set; }
    public string ColorClass { get; set; } = "bg-primary"; // Örn: bg-primary, bg-success, bg-warning
}

/// <summary>
/// Tablo satırlarına ve kompakt alanlara sığacak boyutta çoklu kategorik ilerleme çubuğu ve göstergesi.
/// </summary>
public partial class ThMiniProgressBar : TurboComponentBase
{
    [Parameter] public List<ProgressSegment> Segments { get; set; } = new();
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public bool ShowLegend { get; set; } = true;

    private string RootClass => Cx(
        "th-mini-progress-bar",
        SizeClass(Size, "th-mini-progress-bar"),
        Class);

    private readonly List<DrawSegment> _drawSegments = new();

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        CalculateSegments();
    }

    private void CalculateSegments()
    {
        _drawSegments.Clear();
        if (Segments == null || !Segments.Any()) return;

        var total = Segments.Sum(s => s.Value);
        if (total <= 0) return;

        foreach (var seg in Segments)
        {
            var percentage = (seg.Value / total) * 100;
            _drawSegments.Add(new DrawSegment
            {
                Label = seg.Label,
                Value = seg.Value,
                Percentage = percentage,
                ColorClass = seg.ColorClass
            });
        }
    }

    private class DrawSegment
    {
        public string Label { get; set; } = "";
        public double Value { get; set; }
        public double Percentage { get; set; }
        public string ColorClass { get; set; } = "";
    }
}
