using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Button;

/// <summary>th-btn sarmalayıcısı. Boyut/varyant sabit sınıf eşlemesiyle verilir (interpolasyon yok).</summary>
public partial class ThButton
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Primary;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public string? Icon { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Loading { get; set; }
    [Parameter] public bool Block { get; set; }
    [Parameter] public bool IconOnly { get; set; }
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private string RootClass => Cx(
        "th-btn",
        VariantClass,
        SizeClass(Size, "th-btn"),
        Block ? "th-btn--block" : null,
        IconOnly ? "th-btn--icon" : null,
        Loading ? "th-btn--loading" : null,
        Class);

    private string VariantClass => Variant switch
    {
        ButtonVariant.Secondary => "th-btn--secondary",
        ButtonVariant.Outline => "th-btn--outline",
        ButtonVariant.Ghost => "th-btn--ghost",
        ButtonVariant.Subtle => "th-btn--subtle",
        ButtonVariant.Danger => "th-btn--danger",
        ButtonVariant.Success => "th-btn--success",
        _ => "th-btn--primary"
    };
}
