using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Base;
using Turbohesap.Web.Components.ContextMenu;

namespace Turbohesap.Web.Components.Dropdown;

/// <summary>
/// ThDropdownItem: Açılır menülerdeki tıklanabilir eylem elemanı.
/// </summary>
public partial class ThDropdownItem : TurboComponentBase
{
    [CascadingParameter] public ThDropdown? ParentDropdown { get; set; }
    [CascadingParameter] public ThContextMenu? ParentContextMenu { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Icon { get; set; }
    [Parameter] public string? Shortcut { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Danger { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private string ItemClass => Cx(
        "th-dropdown-item",
        Disabled ? "th-dropdown-item--disabled" : null,
        Danger ? "th-dropdown-item--danger" : null,
        Class
    );

    private async Task HandleClickAsync(MouseEventArgs e)
    {
        if (Disabled) return;

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }

        if (ParentDropdown != null)
        {
            await ParentDropdown.CloseAsync();
        }

        if (ParentContextMenu != null)
        {
            await ParentContextMenu.CloseAsync();
        }
    }
}
