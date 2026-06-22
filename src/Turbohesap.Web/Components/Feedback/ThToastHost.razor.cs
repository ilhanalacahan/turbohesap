using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbohesap.Web.Models;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>Toast bölgesini bir kez render eder; süresi dolan veya eylemleri gerçekleşen toast'ları yönetir.</summary>
public partial class ThToastHost : IDisposable
{
    private readonly HashSet<Guid> _scheduled = [];

    protected override void OnInitialized() => Toasts.OnChange += HandleChange;

    private async void HandleChange()
    {
        await InvokeAsync(StateHasChanged);

        foreach (var toast in Toasts.Toasts.ToList())
        {
            if (!toast.Fixed && toast.DurationMs > 0 && _scheduled.Add(toast.Id))
            {
                var id = toast.Id;
                _ = Task.Delay(toast.DurationMs).ContinueWith(_ =>
                {
                    _scheduled.Remove(id);
                    Toasts.Remove(id);
                }, TaskScheduler.Default);
            }
        }
    }

    private void ExecuteAction(ToastMessage toast, ToastAction action)
    {
        try
        {
            action.OnClick();
        }
        catch
        {
            // Fail silent
        }
        Toasts.Remove(toast.Id);
    }

    public void Dispose() => Toasts.OnChange -= HandleChange;
}
