using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>
/// ThDrawer bileşeni: code-behind sınıfı.
/// Pozisyonlanabilir, genişletilebilir, dinamik sayfa sarmalayabilen yan panel (drawer) bileşeni.
/// </summary>
public partial class ThDrawer : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Subtitle { get; set; }
    
    [Parameter] public DrawerPosition Position { get; set; } = DrawerPosition.Right;
    [Parameter] public DrawerSize Size { get; set; } = DrawerSize.Md;
    [Parameter] public bool Expandable { get; set; } = false;
    [Parameter] public bool CloseOnOverlay { get; set; } = true;
    
    [Parameter] public ModalVariant Variant { get; set; } = ModalVariant.Default;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }

    private ElementReference _drawerRef;
    private ElementReference _resizeHandleRef;
    private IJSObjectReference? _resizableHandler;

    private Task CloseAsync() => VisibleChanged.InvokeAsync(false);

    private Task OnOverlayClick() => CloseOnOverlay ? CloseAsync() : Task.CompletedTask;

    private string OverlayClass => Cx(
        "th-drawer-overlay",
        Position switch
        {
            DrawerPosition.Left => "th-drawer-overlay--left",
            DrawerPosition.Top => "th-drawer-overlay--top",
            DrawerPosition.Bottom => "th-drawer-overlay--bottom",
            _ => "th-drawer-overlay--right"
        }
    );

    private string RootClass => Cx(
        "th-drawer",
        Position switch
        {
            DrawerPosition.Left => "th-drawer--left",
            DrawerPosition.Top => "th-drawer--top",
            DrawerPosition.Bottom => "th-drawer--bottom",
            _ => "th-drawer--right"
        },
        Size switch
        {
            DrawerSize.Sm => "th-drawer--sm",
            DrawerSize.Lg => "th-drawer--lg",
            DrawerSize.Xl => "th-drawer--xl",
            DrawerSize.Fullscreen => "th-drawer--fullscreen",
            _ => "th-drawer--md"
        },
        Variant switch
        {
            ModalVariant.Primary => "th-modal--variant-primary",
            ModalVariant.Secondary => "th-modal--variant-secondary",
            ModalVariant.Success => "th-modal--variant-success",
            ModalVariant.Warning => "th-modal--variant-warning",
            ModalVariant.Danger => "th-modal--variant-danger",
            ModalVariant.Info => "th-modal--variant-info",
            _ => "th-modal--variant-default"
        },
        Class
    );

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Visible)
        {
            try
            {
                await JS.InvokeVoidAsync("turbohesap.lockScroll");
            }
            catch
            {
                // Fail silent
            }

            if (Expandable && _resizableHandler == null)
            {
                try
                {
                    _resizableHandler = await JS.InvokeAsync<IJSObjectReference>(
                        "turbohesap.initResizableDrawer", 
                        _drawerRef, 
                        _resizeHandleRef, 
                        Position.ToString().ToLowerInvariant()
                    );
                }
                catch
                {
                    // Fail silent
                }
            }
        }
        else
        {
            await CleanupResizableAsync();
            try
            {
                await JS.InvokeVoidAsync("turbohesap.unlockScroll");
                await JS.InvokeVoidAsync("turbohesap.clearDrawerStyle", _drawerRef);
            }
            catch
            {
                // Fail silent
            }
        }
    }

    private async Task CleanupResizableAsync()
    {
        if (_resizableHandler != null)
        {
            try
            {
                await _resizableHandler.InvokeVoidAsync("dispose");
                await _resizableHandler.DisposeAsync();
            }
            catch
            {
                // Fail silent
            }
            _resizableHandler = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await CleanupResizableAsync();
        try
        {
            await JS.InvokeVoidAsync("turbohesap.unlockScroll");
            await JS.InvokeVoidAsync("turbohesap.clearDrawerStyle", _drawerRef);
        }
        catch
        {
            // Fail silent
        }
        GC.SuppressFinalize(this);
    }

    private string ResizeHandleClass => Cx(
        "th-drawer__resize-handle",
        Position switch
        {
            DrawerPosition.Left => "th-drawer__resize-handle--left",
            DrawerPosition.Top => "th-drawer__resize-handle--top",
            DrawerPosition.Bottom => "th-drawer__resize-handle--bottom",
            _ => "th-drawer__resize-handle--right"
        }
    );
}
