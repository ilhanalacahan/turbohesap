using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Turbohesap.Web.Components.Data;
using Turbohesap.Web.Components.Card;
using Turbohesap.Web.Components.Progress;

namespace Turbohesap.Web.Pages.Showcase;

/// <summary>
/// ThKpiCard, ThHeatmapCalendar, ThProcessFlow, ThFunnelChart ve ThMiniProgressBar analitik bileşenleri için test vitrini.
/// </summary>
public partial class AnalyticsShowcase : ComponentBase
{
    [Inject] private Services.ToastService ToastService { get; set; } = default!;

    // KPI Kartları için Sparkline verileri
    private List<decimal> _kpiSalesData = new() { 12000, 15000, 14000, 19000, 25000, 32000, 45230 };
    private List<decimal> _kpiExpensesData = new() { 8200, 9100, 8500, 12000, 15000, 14200, 13800 };
    private List<decimal> _kpiStockData = new() { 140, 145, 138, 125, 122, 130, 158 };

    // Aktivite Heatmap verileri (son 3 ay için mock)
    private Dictionary<DateTime, int> _heatmapActivities = GenerateHeatmapMockData();

    // Süreç Akış Şeridi verileri
    private List<ProcessStep> _processFlowSteps = new()
    {
        new() { Id = "1", Label = "Sipariş Alındı", Description = "Müşteri siparişi doğrulandı", Status = StepStatus.Completed, Icon = "fa-solid fa-cart-shopping" },
        new() { Id = "2", Label = "Ödeme Onaylandı", Description = "Banka provizyonu alındı", Status = StepStatus.Completed, Icon = "fa-solid fa-credit-card" },
        new() { Id = "3", Label = "Hazırlanıyor", Description = "Depo rafından toplama aşaması", Status = StepStatus.Active, Icon = "fa-solid fa-box-open" },
        new() { Id = "4", Label = "Kargoya Verildi", Description = "Yurtiçi Kargo Takip No: 34509", Status = StepStatus.Pending, Icon = "fa-solid fa-truck" },
        new() { Id = "5", Label = "Teslim Edildi", Description = "Alıcı imzası bekleniyor", Status = StepStatus.Pending, Icon = "fa-solid fa-house-chimney" }
    };

    // Dönüşüm Hunisi verileri
    private List<FunnelStage> _funnelStages = new()
    {
        new() { Label = "Teklif Verildi", Value = 120, Description = "Sistem üzerinden iletilen teklifler" },
        new() { Label = "Pazarlık Süreci", Value = 85, Description = "Revize edilen veya görüşülenler" },
        new() { Label = "Sözleşme Aşaması", Value = 55, Description = "Taslak sözleşme gönderilenler" },
        new() { Label = "Sipariş / Fatura", Value = 38, Description = "Başarıyla satışla sonuçlananlar" }
    };

    // Mini Segmentli Bar verileri
    private List<ProgressSegment> _miniProgressSegments = new()
    {
        new() { Label = "Hammaddeler", Value = 450, ColorClass = "bg-primary" },
        new() { Label = "Mamuller", Value = 250, ColorClass = "bg-success" },
        new() { Label = "Yarı Mamuller", Value = 180, ColorClass = "bg-warning" },
        new() { Label = "Fireler", Value = 50, ColorClass = "bg-danger" }
    };

    private static Dictionary<DateTime, int> GenerateHeatmapMockData()
    {
        var data = new Dictionary<DateTime, int>();
        var rand = new Random();
        var today = DateTime.Today;
        // Son 120 gün için rastgele veriler üretelim
        for (int i = 0; i < 120; i++)
        {
            var date = today.AddDays(-i);
            // Hafta sonları daha az aktivite, bazı günler sıfır
            var dayVal = rand.Next(0, 12);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                dayVal = rand.Next(0, 3);
            }
            data[date] = dayVal;
        }
        return data;
    }

    private void HandleStepClick(ProcessStep step)
    {
        ToastService.Info($"Tıklanan Süreç: {step.Label} (Durum: {step.Status})", "İş Akışı");
    }

    private void CompleteStep3()
    {
        _processFlowSteps[2].Status = StepStatus.Completed;
        _processFlowSteps[3].Status = StepStatus.Active;
        _processFlowSteps = new List<ProcessStep>(_processFlowSteps);
        ToastService.Success("3. Adım tamamlandı, 4. adım aktif hale getirildi.", "Süreç Simülasyonu");
    }

    private void FailStep3()
    {
        _processFlowSteps[2].Status = StepStatus.Failed;
        _processFlowSteps = new List<ProcessStep>(_processFlowSteps);
        ToastService.Error("3. Adım başarısız olarak işaretlendi.", "Süreç Simülasyonu");
    }

    private void ResetSteps()
    {
        _processFlowSteps[0].Status = StepStatus.Completed;
        _processFlowSteps[1].Status = StepStatus.Completed;
        _processFlowSteps[2].Status = StepStatus.Active;
        _processFlowSteps[3].Status = StepStatus.Pending;
        _processFlowSteps[4].Status = StepStatus.Pending;
        _processFlowSteps = new List<ProcessStep>(_processFlowSteps);
        ToastService.Info("Süreç adımları başlangıç durumuna sıfırlandı.", "Süreç Simülasyonu");
    }

    private void UpdateFunnelStageValue(int index, double val)
    {
        if (index >= 0 && index < _funnelStages.Count)
        {
            _funnelStages[index].Value = Math.Max(0, val);
            _funnelStages = new List<FunnelStage>(_funnelStages); 
        }
    }

    private void UpdateSegmentValue(int index, double val)
    {
        if (index >= 0 && index < _miniProgressSegments.Count)
        {
            _miniProgressSegments[index].Value = Math.Max(0, val);
            _miniProgressSegments = new List<ProgressSegment>(_miniProgressSegments);
        }
    }

    private HeatmapColor _heatmapColor = HeatmapColor.Primary;

    private void ChangeHeatmapColor(HeatmapColor color)
    {
        _heatmapColor = color;
        ToastService.Info($"Heatmap renk teması '{color}' olarak değiştirildi.", "Tema Değişikliği");
    }

    private void GenerateRandomHeatmapData()
    {
        _heatmapActivities = GenerateHeatmapMockData();
        ToastService.Success("Rastgele aktivite yoğunluk verileri yeniden üretildi.", "Yoğunluk Matrisi");
    }

    private void ClearHeatmapData()
    {
        _heatmapActivities = new Dictionary<DateTime, int>();
        ToastService.Warning("Tüm aktivite verileri temizlendi.", "Yoğunluk Matrisi");
    }
}
