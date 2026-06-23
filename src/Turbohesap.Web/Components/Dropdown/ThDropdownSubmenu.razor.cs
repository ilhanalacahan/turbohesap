using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Dropdown;

/// <summary>
/// ThDropdownSubmenu: Alt menü (nested/hover submenu) tetikleyici ve kapsayıcı elemanı.
/// </summary>
public partial class ThDropdownSubmenu : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public string Header { get; set; } = string.Empty;
    [Parameter] public string? Icon { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private ElementReference _itemRef;
    private ElementReference _submenuRef;
    private IJSObjectReference? _jsHandler;
    private bool _isSubmenuOpen = false;

    private string ItemClass => Cx(
        "th-dropdown-item th-dropdown-item--submenu",
        Disabled ? "th-dropdown-item--disabled" : null,
        Class
    );

    private async Task OpenSubmenuAsync()
    {
        if (Disabled) return;
        _isSubmenuOpen = true;
        StateHasChanged();
    }

    private async Task CloseSubmenuAsync()
    {
        _isSubmenuOpen = false;
        StateHasChanged();
        await CleanupJSAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isSubmenuOpen && _jsHandler == null)
        {
            try
            {
                _jsHandler = await JS.InvokeAsync<IJSObjectReference>("turbohesap.menu.initSubmenu", _itemRef, _submenuRef);
            }
            catch
            {
                // Fail silent
            }
        }
    }

    private async Task CleanupJSAsync()
    {
        if (_jsHandler != null)
        {
            try
            {
                await _jsHandler.InvokeVoidAsync("dispose");
                await _jsHandler.DisposeAsync();
            }
            catch
            {
                // Fail silent
            }
            _jsHandler = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await CleanupJSAsync();
        GC.SuppressFinalize(this);
    }
}
