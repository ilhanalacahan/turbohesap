using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Card;

/// <summary>
/// Genel sarmalayıcı kart / panel (th-card) bileşeni.
/// </summary>
public partial class ThCard : TurboComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public string? Icon { get; set; }
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public RenderFragment? HeaderActions { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }
    [Parameter] public bool NoPadding { get; set; }

    private string RootClass => Cx("th-card", Class);
}
