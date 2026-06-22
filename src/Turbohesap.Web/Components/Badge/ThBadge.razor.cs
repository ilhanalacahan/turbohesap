using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Badge;

public enum BadgeVariant { Neutral, Primary, Success, Warning, Danger, Info }

/// <summary>
/// Erişilebilir rozet (th-badge) bileşeni.
/// </summary>
public partial class ThBadge : TurboComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public BadgeVariant Variant { get; set; } = BadgeVariant.Neutral;
    [Parameter] public bool ShowDot { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string RootClass => Cx(
        "th-badge",
        Variant switch
        {
            BadgeVariant.Primary => "th-badge--primary",
            BadgeVariant.Success => "th-badge--success",
            BadgeVariant.Warning => "th-badge--warning",
            BadgeVariant.Danger => "th-badge--danger",
            BadgeVariant.Info => "th-badge--info",
            _ => "th-badge--neutral"
        },
        SizeClass(Size, "th-badge"),
        Class);
}
