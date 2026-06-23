using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Turbohesap.Web.Components.Data;
using Turbohesap.Web.Components.Progress;

namespace Turbohesap.Web.Pages.Showcase;

/// <summary>
/// ThCard, ThBadge, ThAvatar, ThProgress, ThStockAlertLevel, ThCapacityGauge ve ThGanttRow bileşenleri için test vitrini.
/// </summary>
public partial class DisplayShowcase : ComponentBase
{
    private double _progressValue = 45;

    // Kadran (Gauge) için interaktif veri
    private double _gaugeValue = 145;
    private double _gaugeMax = 200;

    private void IncreaseProgress()
    {
        _progressValue = Math.Min(100, _progressValue + 10);
    }

    private void DecreaseProgress()
    {
        _progressValue = Math.Max(0, _progressValue - 10);
    }

    private void IncreaseGauge()
    {
        _gaugeValue = Math.Min(_gaugeMax, _gaugeValue + 15);
    }

    private void DecreaseGauge()
    {
        _gaugeValue = Math.Max(0, _gaugeValue - 15);
    }

    // Gantt Şeridi için mock veriler
    private List<GanttSegment> _productionOrderGantt = new()
    {
        new() { Label = "Hazırlık", StartPercent = 0, EndPercent = 15, Status = GanttSegmentStatus.Completed, Icon = "fa-solid fa-screwdriver-wrench", Tooltip = "Hammadde hazırlığı ve kalıp ayarları yapıldı." },
        new() { Label = "Kesim (Laser)", StartPercent = 15, EndPercent = 45, Status = GanttSegmentStatus.Completed, Icon = "fa-solid fa-scissors", Tooltip = "Metal sac kesim aşaması tamamlandı." },
        new() { Label = "Büküm (Press)", StartPercent = 45, EndPercent = 75, Status = GanttSegmentStatus.Active, Icon = "fa-solid fa-wave-square", Tooltip = "Büküm ve form verme işlemi devam ediyor. Operatör: Ahmet Y." },
        new() { Label = "Boya & Montaj", StartPercent = 75, EndPercent = 100, Status = GanttSegmentStatus.Pending, Icon = "fa-solid fa-palette", Tooltip = "Statik toz boya ve son montaj aşaması." }
    };

    private List<GanttSegment> _machineUtilizationGantt = new()
    {
        new() { Label = "CNC-01 Çalışıyor", StartPercent = 0, EndPercent = 40, Status = GanttSegmentStatus.Completed, Icon = "fa-solid fa-play", Tooltip = "İş Emri #3098 aktif olarak işlendi." },
        new() { Label = "Arıza / Duruş", StartPercent = 40, EndPercent = 60, Status = GanttSegmentStatus.Delayed, Icon = "fa-solid fa-triangle-exclamation", Tooltip = "Plansız motor harareti arızası ve bakım duruşu." },
        new() { Label = "CNC-01 Çalışıyor", StartPercent = 60, EndPercent = 90, Status = GanttSegmentStatus.Active, Icon = "fa-solid fa-gear", Tooltip = "Yedek iş emri #3102 devreye alındı." },
        new() { Label = "Boşta / Bekleme", StartPercent = 90, EndPercent = 100, Status = GanttSegmentStatus.Pending, Tooltip = "Sıradaki iş emri için onay bekleniyor." }
    };
}
