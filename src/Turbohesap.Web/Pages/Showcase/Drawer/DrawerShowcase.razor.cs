using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Feedback;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.Drawer;

/// <summary>
/// Showcase sayfasındaki Drawer testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class DrawerShowcase : ComponentBase
{
    [Inject] private ThDrawerService DrawerService { get; set; } = default!;
    [Inject] private ToastService Toasts { get; set; } = default!;

    private bool _inlineDrawerOpen;
    private DrawerPosition _inlineDrawerPosition = DrawerPosition.Right;
    private DrawerSize _inlineDrawerSize = DrawerSize.Md;
    private bool _inlineDrawerExpandable;
    private ModalVariant _inlineDrawerVariant = ModalVariant.Default;

    private void OpenInlineDrawer(DrawerPosition position, DrawerSize size, bool expandable, ModalVariant variant)
    {
        _inlineDrawerPosition = position;
        _inlineDrawerSize = size;
        _inlineDrawerExpandable = expandable;
        _inlineDrawerVariant = variant;
        _inlineDrawerOpen = true;
    }

    private async Task OpenServiceDrawer(DrawerPosition position)
    {
        var options = new DrawerOptions
        {
            Position = position,
            Size = DrawerSize.Md,
            Expandable = true,
            Variant = ModalVariant.Primary
        };

        var parameters = new Dictionary<string, object>
        {
            ["InitialValue"] = $"Varsayılan Vitrin Metni ({position})"
        };

        var drawerRef = DrawerService.Show<ShowcaseDrawerContent>("Özel Form Yan Paneli", parameters, options);
        var result = await drawerRef.Result;

        if (!result.Cancelled)
        {
            Toasts.Success($"Yan panelden gelen veri: {result.Data}");
        }
        else
        {
            Toasts.Info("Yan panel iptal edildi.");
        }
    }
}
