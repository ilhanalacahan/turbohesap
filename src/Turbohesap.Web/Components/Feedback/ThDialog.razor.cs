using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>th-dialog/modal sarmalayıcısı. Boyut sabit sınıf eşlemesiyle verilir.</summary>
public partial class ThDialog
{
    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public DialogSize Size { get; set; } = DialogSize.Md;
    [Parameter] public bool CloseOnOverlay { get; set; } = true;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }

    private string RootClass => Cx("th-dialog", Size switch
    {
        DialogSize.Sm => "th-dialog--sm",
        DialogSize.Lg => "th-dialog--lg",
        DialogSize.Xl => "th-dialog--xl",
        _ => "th-dialog--md"
    }, Class);

    private Task CloseAsync() => VisibleChanged.InvokeAsync(false);

    private Task OnOverlayClick() => CloseOnOverlay ? CloseAsync() : Task.CompletedTask;
}
