using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Tooltip;

public enum TooltipPosition { Top, Bottom, Left, Right }

/// <summary>
/// Bir elementin üzerine gelindiğinde (hover) açılan bilgilendirme kutusu (ThTooltip).
/// </summary>
public partial class ThTooltip : TurboComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Text { get; set; } = string.Empty;
    [Parameter] public TooltipPosition Position { get; set; } = TooltipPosition.Top;

    private string RootClass => Cx("th-tooltip-wrap", Class);

    private string TooltipClass => Cx(
        "th-tooltip",
        Position switch
        {
            TooltipPosition.Bottom => "th-tooltip--bottom",
            TooltipPosition.Left => "th-tooltip--left",
            TooltipPosition.Right => "th-tooltip--right",
            _ => "th-tooltip--top"
        });
}
