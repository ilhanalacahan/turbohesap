using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Alert;

public enum AlertVariant { Info, Success, Warning, Danger }

/// <summary>
/// ThAlert: Inline uyarı ve bildirim paneli bileşeni.
/// </summary>
public partial class ThAlert : TurboComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public AlertVariant Variant { get; set; } = AlertVariant.Info;
    [Parameter] public bool ShowIcon { get; set; } = true;
    [Parameter] public bool Closable { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private bool _visible = true;

    private void Close()
    {
        _visible = false;
        _ = OnClose.InvokeAsync();
        StateHasChanged();
    }

    private string RootClass => Cx(
        "th-alert",
        Variant switch
        {
            AlertVariant.Success => "th-alert--success",
            AlertVariant.Warning => "th-alert--warning",
            AlertVariant.Danger => "th-alert--danger",
            _ => "th-alert--info"
        },
        Class);

    private string IconClass => Variant switch
    {
        AlertVariant.Success => "fa-solid fa-circle-check",
        AlertVariant.Warning => "fa-solid fa-triangle-exclamation",
        AlertVariant.Danger => "fa-solid fa-circle-xmark",
        _ => "fa-solid fa-circle-info"
    };
}
