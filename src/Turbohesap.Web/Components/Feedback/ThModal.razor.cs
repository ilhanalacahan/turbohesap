using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>
/// ThModal component: code-behind class.
/// DESIGN.md/AGENTS.md kuralları ile %100 uyumlu C# mantık katmanı.
/// </summary>
public partial class ThModal : IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public ModalSize Size { get; set; } = ModalSize.Md;
    [Parameter] public bool Draggable { get; set; } = false;
    [Parameter] public bool Fullscreen { get; set; } = false;
    [Parameter] public bool Maximizable { get; set; } = false;
    [Parameter] public bool CloseOnOverlay { get; set; } = true;
    [Parameter] public ModalVariant Variant { get; set; } = ModalVariant.Default;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }

    private ElementReference _dialogRef;
    private ElementReference _headerRef;
    private IJSObjectReference? _draggableHandler;
    private bool _maximized;
    private bool _prevVisible;

    private string RootClass => Cx(
        "th-modal",
        Size switch
        {
            ModalSize.Sm => "th-modal--sm",
            ModalSize.Lg => "th-modal--lg",
            ModalSize.Xl => "th-modal--xl",
            _ => "th-modal--md"
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
        Fullscreen || _maximized ? "th-modal--fullscreen" : null,
        Draggable && !_maximized && !Fullscreen ? "th-modal--draggable" : null,
        Class
    );

    protected override async Task OnParametersSetAsync()
    {
        if (Visible && !_prevVisible)
        {
            _maximized = false; // Reset maximized state when re-opened
        }
        _prevVisible = Visible;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Visible)
        {
            if (Draggable && _draggableHandler == null && !_maximized && !Fullscreen)
            {
                try
                {
                    _draggableHandler = await JS.InvokeAsync<IJSObjectReference>("turbohesap.initDraggable", _dialogRef, _headerRef);
                }
                catch
                {
                    // Fail silent if JS environment isn't fully ready
                }
            }
            else if ((!Draggable || _maximized || Fullscreen) && _draggableHandler != null)
            {
                await CleanupDraggableAsync();
            }
        }
        else
        {
            await CleanupDraggableAsync();
        }
    }

    private void ToggleMaximize()
    {
        _maximized = !_maximized;
    }

    private Task CloseAsync() => VisibleChanged.InvokeAsync(false);

    private Task OnOverlayClick() => CloseOnOverlay ? CloseAsync() : Task.CompletedTask;

    private async Task CleanupDraggableAsync()
    {
        if (_draggableHandler != null)
        {
            try
            {
                await _draggableHandler.InvokeVoidAsync("dispose");
                await _draggableHandler.DisposeAsync();
            }
            catch
            {
                // Fail silent during teardown
            }
            _draggableHandler = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await CleanupDraggableAsync();
        GC.SuppressFinalize(this);
    }
}
