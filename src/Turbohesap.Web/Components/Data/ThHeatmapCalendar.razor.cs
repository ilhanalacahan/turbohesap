using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

public enum HeatmapColor
{
    Primary,
    Success,
    Warning,
    Danger
}

/// <summary>
/// GitHub katkı tablosu stilinde, süreç yoğunluklarını ve aktiviteleri gün bazında görselleştiren takvim matrisi.
/// </summary>
public partial class ThHeatmapCalendar : TurboComponentBase
{
    [Parameter] public string Title { get; set; } = "Aktivite Yoğunluk Matrisi";
    [Parameter] public Dictionary<DateTime, int>? ActivityData { get; set; }
    [Parameter] public int MaxActivityValue { get; set; } = 10;
    [Parameter] public HeatmapColor ColorVariant { get; set; } = HeatmapColor.Primary;
    [Parameter] public int MonthCount { get; set; } = 3; // Gösterilecek ay sayısı

    private readonly List<DateTime> _days = new();
    private readonly List<string> _monthLabels = new();

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        GenerateCalendarDays();
    }

    private void GenerateCalendarDays()
    {
        _days.Clear();
        _monthLabels.Clear();

        var today = DateTime.Today;
        // Gösterilecek başlangıç tarihini ayarla
        var startMonthDate = new DateTime(today.Year, today.Month, 1).AddMonths(-(MonthCount - 1));
        
        // Pazar gününden başlaması için (veya Pazartesi) takvim hizalaması
        var dayOfWeek = (int)startMonthDate.DayOfWeek;
        var offset = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
        var startDate = startMonthDate.AddDays(-offset);

        var endDate = today;
        var endDayOfWeek = (int)endDate.DayOfWeek;
        var endOffset = endDayOfWeek == 0 ? 0 : 7 - endDayOfWeek;
        endDate = endDate.AddDays(endOffset);

        var current = startDate;
        while (current <= endDate)
        {
            _days.Add(current);
            current = current.AddDays(1);
        }

        // Ay başlıklarını toplayalım
        var temp = startMonthDate;
        while (temp <= endDate)
        {
            _monthLabels.Add(temp.ToString("MMMM"));
            temp = temp.AddMonths(1);
        }
    }

    private string RootClass => Cx(
        "th-heatmap-calendar",
        $"th-heatmap-calendar--{ColorVariant.ToString().ToLowerInvariant()}",
        Class);

    private int GetIntensity(DateTime date)
    {
        if (ActivityData == null || !ActivityData.TryGetValue(date, out var val))
            return 0;

        if (val <= 0) return 0;
        if (val >= MaxActivityValue) return 4;

        var ratio = (float)val / MaxActivityValue;
        if (ratio <= 0.25f) return 1;
        if (ratio <= 0.50f) return 2;
        if (ratio <= 0.75f) return 3;
        return 4;
    }

    private string GetCellClass(DateTime date)
    {
        var intensity = GetIntensity(date);
        var isToday = date == DateTime.Today;
        return Cx(
            "th-heatmap-cell",
            $"th-heatmap-cell--level-{intensity}",
            isToday ? "th-heatmap-cell--today" : ""
        );
    }
}
