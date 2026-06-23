using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

public enum StepStatus
{
    Pending,
    Active,
    Completed,
    Failed
}

public class ProcessStep
{
    public string Id { get; set; } = "";
    public string Label { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public StepStatus Status { get; set; } = StepStatus.Pending;
}

/// <summary>
/// Adımlı iş süreçlerini, onay mekanizmalarını veya sipariş durumlarını yatay/dikey çizgilerle görselleştiren süreç akış şeridi.
/// </summary>
public partial class ThProcessFlow : TurboComponentBase
{
    [Parameter] public List<ProcessStep> Steps { get; set; } = new();
    [Parameter] public EventCallback<ProcessStep> OnStepClick { get; set; }
    [Parameter] public bool Vertical { get; set; } = false;

    private string RootClass => Cx(
        "th-process-flow",
        Vertical ? "th-process-flow--vertical" : "th-process-flow--horizontal",
        Class);

    private async Task HandleStepClick(ProcessStep step)
    {
        if (OnStepClick.HasDelegate)
        {
            await OnStepClick.InvokeAsync(step);
        }
    }

    private string GetStepClass(ProcessStep step)
    {
        return Cx(
            "th-process-step",
            $"th-process-step--{step.Status.ToString().ToLowerInvariant()}",
            OnStepClick.HasDelegate ? "th-process-step--clickable" : ""
        );
    }
}
