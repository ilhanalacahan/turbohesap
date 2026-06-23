using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Card;

/// <summary>
/// Gelişmiş veri sunumu için entegre mikro çizgi grafikli (Sparkline) KPI ve Metrik kartı.
/// </summary>
public partial class ThKpiCard : TurboComponentBase
{
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public string Value { get; set; } = "";
    [Parameter] public string Subtitle { get; set; } = "";
    [Parameter] public decimal? TrendValue { get; set; } // Örn: 0.12 (%12 artış)
    [Parameter] public List<decimal>? SparklineData { get; set; }
    [Parameter] public string Icon { get; set; } = "";
    [Parameter] public string TrendLabel { get; set; } = "";

    private string RootClass => Cx(
        "th-kpi-card",
        TrendValue.HasValue ? (TrendValue.Value >= 0 ? "th-kpi-card--trend-up" : "th-kpi-card--trend-down") : "",
        Class);

    private string SvgPath => GenerateSvgPath();

    private string GenerateSvgPath()
    {
        if (SparklineData == null || SparklineData.Count < 2) return "";

        var min = SparklineData.Min();
        var max = SparklineData.Max();
        var range = max - min;
        if (range == 0) range = 1;

        var width = 120;
        var height = 36;
        var padding = 2;

        var points = new List<string>();
        for (int i = 0; i < SparklineData.Count; i++)
        {
            var val = SparklineData[i];
            var x = (float)i / (SparklineData.Count - 1) * (width - padding * 2) + padding;
            var y = height - ((float)(val - min) / (float)range * (height - padding * 2) + padding);
            points.Add($"{x.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)},{y.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}");
        }

        return string.Join(" ", points);
    }
}
