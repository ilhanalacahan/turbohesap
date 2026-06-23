using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbohesap.Web.Models;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.Toast;

/// <summary>
/// Showcase sayfasındaki Toast testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class ToastShowcase : ComponentBase
{
    [Inject] private ToastService Toasts { get; set; } = default!;

    private void ShowToastWithActions()
    {
        var actions = new List<ToastAction>
        {
            new() 
            { 
                Text = "Geri Al", 
                Icon = "fa-solid fa-rotate-left", 
                VariantClass = "th-btn--subtle",
                OnClick = () => Toasts.Success("Silme işlemi geri alındı!") 
            },
            new()
            {
                Text = "Yoksay",
                OnClick = () => Toasts.Info("İşlem yoksayıldı.")
            }
        };

        Toasts.Warning("Müşteri kaydı silindi.", "Veri Silindi", durationMs: 8000, actions: actions);
    }

    private void ShowToastPosition(ToastPosition position)
    {
        Toasts.Info($"Bu bildirim {position} konumunda gösteriliyor.", "Pozisyon Testi", position: position);
    }

    private void ShowStickyToast()
    {
        Toasts.Warning("Bu bildirim siz kapatana kadar ekranda kalır.", "Sabit Bildirim", isFixed: true);
    }

    private void ShowToastWithProgress()
    {
        Toasts.Success("Yedekleme işlemi arka planda yürütülüyor.", "Yedekleniyor", showProgress: true, durationMs: 5000);
    }

    private async Task ShowToastDynamicProgress()
    {
        var toast = new ToastMessage
        {
            Message = "Rapor hazırlanıyor... %0",
            Title = "Rapor Oluşturma",
            Level = ToastLevel.Info,
            ShowProgress = true,
            Progress = 0,
            Fixed = true
        };
        
        Toasts.Show(toast);

        for (int i = 0; i <= 100; i += 20)
        {
            await Task.Delay(400);
            toast.Progress = i;
            toast.Message = i < 100 ? $"Veriler derleniyor... %{i}" : "Rapor hazır!";
            
            if (i == 100)
            {
                toast.Level = ToastLevel.Success;
                toast.Fixed = false;
                toast.DurationMs = 3000;
            }
            Toasts.NotifyChanged();
        }
    }
}
