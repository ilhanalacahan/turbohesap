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

        const int stageHeight = 45; // Her aşamanın yüksekliği
        const int gap = 6;          // Aşamalar arasındaki boşluk
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

            var y1 = i * (stageHeight + gap);
            var y2 = y1 + stageHeight;

            var x1 = (totalWidth - topWidth) / 2;
            var x2 = x1 + topWidth;
            var x3 = (totalWidth - bottomWidth) / 2;
            var x4 = x3 + bottomWidth;

            var points = $"{x1},{y1} {x2},{y1} {x4},{y2} {x3},{y2}";
            var percentage = (stage.Value / maxValue) * 100;

            _drawStages.Add(new FunnelStageDrawData
            {
                Label = stage.Label,
                Value = stage.Value,
                Description = stage.Description,
                Points = points,
                Percentage = percentage,
                ColorIndex = i % 4
            });
        }
    }

    private class FunnelStageDrawData
    {
        public string Label { get; set; } = "";
        public double Value { get; set; }
        public string Description { get; set; } = "";
        public string Points { get; set; } = "";
        public double Percentage { get; set; }
        public int ColorIndex { get; set; }
    }
}
