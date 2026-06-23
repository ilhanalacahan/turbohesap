using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

public enum GanttSegmentStatus
{
    Completed,
    Active,
    Pending,
    Delayed
}

public class GanttSegment
{
    public string Label { get; set; } = string.Empty;
    public double StartPercent { get; set; }
    public double EndPercent { get; set; }
    public GanttSegmentStatus Status { get; set; } = GanttSegmentStatus.Pending;
    public string? Tooltip { get; set; }
    public string? Icon { get; set; }
}

/// <summary>
/// Mini üretim gantt şeridi (ThGanttRow). İş süreçleri, makine planlama ve sipariş durumlarını görselleştirir.
/// </summary>
public partial class ThGanttRow : TurboComponentBase
{
    [Parameter] public string Label { get; set; } = "İş Süreci";
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public List<GanttSegment> Segments { get; set; } = new();
    [Parameter] public bool ShowTicks { get; set; } = true;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string RootClass => Cx(
        "th-gantt-row",
        SizeClass(Size, "th-gantt-row"),
        Class);

    protected string GetSegmentClass(GanttSegmentStatus status) => status switch
    {
        GanttSegmentStatus.Completed => "th-gantt-segment--completed",
        GanttSegmentStatus.Active => "th-gantt-segment--active",
        GanttSegmentStatus.Delayed => "th-gantt-segment--delayed",
        _ => "th-gantt-segment--pending"
    };

    protected double GetWidth(GanttSegment segment)
    {
        var width = segment.EndPercent - segment.StartPercent;
        return Math.Min(100, Math.Max(0, width));
    }
}
