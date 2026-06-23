using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Dropdown;

/// <summary>
/// ThDropdown: Açılır menü bileşeni.
/// React Portal benzeri çalışarak, overflow: hidden kapsayıcılarından taşar.
/// </summary>
public partial class ThDropdown : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public RenderFragment? Trigger { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private ElementReference _triggerRef;
    private ElementReference _menuRef;
    private IJSObjectReference? _jsHandler;
    private DotNetObjectReference<ThDropdown>? _dotNetRef;
    private bool _isOpen = false;
    private readonly string _id = Guid.NewGuid().ToString();

    private string RootClass => Cx(
        "th-dropdown-trigger",
        Class
    );

    public async Task OpenAsync()
    {
        if (_isOpen) return;
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

    public async Task ToggleAsync()
    {
        if (_isOpen)
        {
            await CloseAsync();
        }
        else
        {
            await OpenAsync();
        }
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
                _jsHandler = await JS.InvokeAsync<IJSObjectReference>("turbohesap.menu.initDropdown", _id, _triggerRef, _menuRef, _dotNetRef);
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
