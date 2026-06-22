using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Tabs;
using Turbohesap.Web.Models;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.Tabs;

/// <summary>
/// Showcase sayfasındaki Tabs testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class TabsShowcase : ComponentBase
{
    [Inject] private ThDialogService DialogService { get; set; } = default!;
    [Inject] private ToastService Toasts { get; set; } = default!;

    protected List<ShowcaseTabItem> _closableTabs = [];

    protected override void OnInitialized()
    {
        _closableTabs = new List<ShowcaseTabItem>
        {
            new() { Id = "tab1", Name = "Doğrudan Kapanır", Icon = "fa-solid fa-file", Content = builder => builder.AddContent(0, "Bu sekme herhangi bir onay almadan doğrudan kapatılabilir.") },
            new() { Id = "tab2", Name = "Onaylı Kapanır", Icon = "fa-solid fa-floppy-disk", Content = builder => builder.AddContent(0, "Bu sekme kapatılmadan önce kullanıcıdan onay isteyecektir.") }
        };
    }

    protected async Task HandleTabClose(ShowcaseTabItem tab, TabCloseEventArgs args)
    {
        if (tab.Id == "tab2")
        {
            var result = await DialogService.ConfirmAsync(
                "Sekmeyi Kapat",
                "Bu sekmeyi kapatmak istediğinize emin misiniz? Kaydedilmemiş değişiklikler kaybolabilir.",
                DialogVariant.Warning,
                "Evet, Kapat",
                "İptal"
            );

            if (result.Status != DialogStatus.Confirmed)
            {
                args.Cancel = true;
                Toasts.Info("Sekme kapatma iptal edildi.");
            }
            else
            {
                _closableTabs.Remove(tab);
                Toasts.Success("Sekme kapatıldı.");
            }
        }
        else
        {
            _closableTabs.Remove(tab);
            Toasts.Success("Sekme kapatıldı.");
        }
    }

    public class ShowcaseTabItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public RenderFragment Content { get; set; } = default!;
    }
}
