using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Popover;

public enum PopoverPosition { Bottom, Top, Left, Right }

/// <summary>
/// ThPopover: Gelişmiş Balon Pencere Bileşeni. Tıklamayla açılır ve form/buton barındırır.
/// </summary>
public partial class ThPopover : TurboComponentBase
{
    [Parameter] public RenderFragment? Trigger { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public bool ShowCloseButton { get; set; } = true;
    [Parameter] public PopoverPosition Position { get; set; } = PopoverPosition.Bottom;

    private bool _isOpen;

    public void Toggle()
    {
        _isOpen = !_isOpen;
        StateHasChanged();
    }

    public void Close()
    {
        if (_isOpen)
        {
            _isOpen = false;
            StateHasChanged();
        }
    }

    private string PopoverClass => Cx(
        "th-popover-content",
        Position switch
        {
            PopoverPosition.Top => "th-popover--top",
            PopoverPosition.Left => "th-popover--left",
            PopoverPosition.Right => "th-popover--right",
            _ => "th-popover--bottom"
        });
}
