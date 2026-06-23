using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.Dropdown;

/// <summary>
/// Showcase sayfasındaki Dropdown ve ContextMenu testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class DropdownShowcase : ComponentBase
{
    [Inject] private ToastService Toasts { get; set; } = default!;

    protected void OnItemClick(string actionName)
    {
        Toasts.Info($"Seçilen Eylem: {actionName}");
    }
}
