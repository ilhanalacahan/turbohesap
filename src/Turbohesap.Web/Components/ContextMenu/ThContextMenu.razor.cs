using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.ContextMenu;

/// <summary>
/// ThContextMenu: Sağ tık (bağlam) menüsü bileşeni.
/// </summary>
public partial class ThContextMenu : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public RenderFragment? TargetZone { get; set; }
    [Parameter] public RenderFragment? Menu { get; set; }

    private ElementReference _menuRef;
    private IJSObjectReference? _jsHandler;
    private DotNetObjectReference<ThContextMenu>? _dotNetRef;
    private bool _isOpen = false;
    private double _x;
    private double _y;
    private readonly string _id = Guid.NewGuid().ToString();

    private string RootClass => Cx(
        "th-contextmenu-zone",
        Class
    );

    private void OnContextMenu(MouseEventArgs e)
    {
        _x = e.ClientX;
        _y = e.ClientY;
        _isOpen = true;
        StateHasChanged();
    }

    public async Task CloseAsync()
    {
        if (!_isOpen) return;
        _isOpen = false;
        StateHasChanged();
        await CleanupJSAsync();
    }

    [JSInvokable]
    public async Task CloseMenu()
    {
        await CloseAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isOpen && _jsHandler == null)
        {
            try
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                _jsHandler = await JS.InvokeAsync<IJSObjectReference>("turbohesap.menu.initContextMenu", _id, _x, _y, _menuRef, _dotNetRef);
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
        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }

    public async ValueTask DisposeAsync()
    {
        await CleanupJSAsync();
        GC.SuppressFinalize(this);
    }
}
