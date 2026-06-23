using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

public class FunnelStage
{
    public string Label { get; set; } = "";
    public double Value { get; set; }
    public string Description { get; set; } = "";
}

/// <summary>
/// SVG tabanlı, aşamalı süreç dönüşüm oranlarını (Örn: tekliften siparişe dönüşüm) görselleştiren dikey huni grafiği.
/// </summary>
public partial class ThFunnelChart : TurboComponentBase
{
    [Parameter] public string Title { get; set; } = "Dönüşüm Hunisi";
    [Parameter] public List<FunnelStage> Stages { get; set; } = new();

    private string RootClass => Cx("th-funnel-chart", Class);

    private readonly List<FunnelStageDrawData> _drawStages = new();

    private const int StageHeight = 45; // Her aşamanın yüksekliği (px = viewBox birimi)
    private const int Gap = 6;          // Aşamalar arasındaki boşluk

    /// <summary>SVG yüksekliği; dikey eşleme etiketlerle 1:1 hizalanır (StageHeight + Gap).</summary>
    private int TotalHeight => _drawStages.Count > 0 ? _drawStages.Count * (StageHeight + Gap) - Gap : 0;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        CalculateDrawData();
    }

    private void CalculateDrawData()
    {
        _drawStages.Clear();
        if (Stages == null || !Stages.Any()) return;

        var maxValue = Stages.First().Value;
        if (maxValue <= 0) maxValue = 1;

        const int totalWidth = 300;

        for (int i = 0; i < Stages.Count; i++)
        {
            var stage = Stages[i];
            var currentVal = stage.Value;
            // Bir sonraki değer (yamuğun alt tabanı için)
            var nextVal = (i + 1 < Stages.Count) ? Stages[i + 1].Value : currentVal * 0.6;

            var topWidth = (currentVal / maxValue) * totalWidth;
            var bottomWidth = (nextVal / maxValue) * totalWidth;

            // Minimum genişlikler
            topWidth = Math.Max(topWidth, 30);
            bottomWidth = Math.Max(bottomWidth, 15);

            var y1 = i * (StageHeight + Gap);
            var y2 = y1 + StageHeight;

            var x1 = (totalWidth - topWidth) / 2;
            var x2 = x1 + topWidth;
            var x3 = (totalWidth - bottomWidth) / 2;
            var x4 = x3 + bottomWidth;

            var points = string.Create(System.Globalization.CultureInfo.InvariantCulture,
                $"{x1:F2},{y1} {x2:F2},{y1} {x4:F2},{y2} {x3:F2},{y2}");
            var percentage = (stage.Value / maxValue) * 100;

            _drawStages.Add(new FunnelStageDrawData
            {
                Label = stage.Label,
                Value = stage.Value,
                Description = stage.Description,
                Points = points,
                Percentage = percentage,
                ColorClass = ColorClassFor(i % 4)
            });
        }
    }

    /// <summary>Aşama sırasını sabit bir renk sınıfına eşler (interpolasyon yok).</summary>
    private static string ColorClassFor(int index) => index switch
    {
        0 => "th-funnel-polygon--color-0",
        1 => "th-funnel-polygon--color-1",
        2 => "th-funnel-polygon--color-2",
        _ => "th-funnel-polygon--color-3"
    };

    private class FunnelStageDrawData
    {
        public string Label { get; set; } = "";
        public double Value { get; set; }
        public string Description { get; set; } = "";
        public string Points { get; set; } = "";
        public double Percentage { get; set; }
        public string ColorClass { get; set; } = "";
    }
}
