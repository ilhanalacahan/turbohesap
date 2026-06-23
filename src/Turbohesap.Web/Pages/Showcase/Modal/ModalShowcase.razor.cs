using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Feedback;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.Modal;

/// <summary>
/// Showcase sayfasındaki Modal testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class ModalShowcase : ComponentBase
{
    [Inject] private ThModalService ModalService { get; set; } = default!;
    [Inject] private ToastService Toasts { get; set; } = default!;

    private bool _inlineModalOpen;
    private bool _draggableModalOpen;

    private async Task OpenServiceModal()
    {
        var options = new ModalOptions
        {
            Size = ModalSize.Md,
            Draggable = true,
            Maximizable = true,
            Variant = ModalVariant.Primary
        };

        var parameters = new Dictionary<string, object>
        {
            ["InitialValue"] = "Varsayılan Vitrin Metni"
        };

        var modalRef = ModalService.Show<ShowcaseModalContent>("Özel Form Modalı", parameters, options);
        var result = await modalRef.Result;

        if (!result.Cancelled)
        {
            Toasts.Success($"Formdan gelen veri: {result.Data}");
        }
        else
        {
            Toasts.Info("Form iptal edildi.");
        }
    }
}
