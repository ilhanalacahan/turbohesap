using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Tabs;

/// <summary>
/// ThTabs: Reusable tab container.
/// </summary>
public partial class ThTabs : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public TabVariant Variant { get; set; } = TabVariant.Line;
    [Parameter] public bool Lazy { get; set; } = false;
    [Parameter] public EventCallback<TabCloseEventArgs> OnTabClose { get; set; }

    protected readonly List<ThTabPanel> _tabs = [];
    protected ThTabPanel? _activeTab;

    private ElementReference _viewportRef;
    private ElementReference _leftArrowRef;
    private ElementReference _rightArrowRef;
    private IJSObjectReference? _scrollHandler;

    private string RootClass => Cx(
        "th-tabs",
        Variant switch
        {
            TabVariant.Pills => "th-tabs--pills",
            TabVariant.Bordered => "th-tabs--bordered",
            TabVariant.Flat => "th-tabs--flat",
            _ => "th-tabs--line"
        },
        Class
    );

    public void RegisterTab(ThTabPanel tab)
    {
        if (!_tabs.Contains(tab))
        {
            _tabs.Add(tab);
            if (_activeTab == null && !tab.Disabled)
            {
                _activeTab = tab;
            }
            StateHasChanged();
            _ = UpdateScrollArrowsAsync();
        }
    }

    public void UnregisterTab(ThTabPanel tab)
    {
        if (_tabs.Remove(tab))
        {
            if (_activeTab == tab)
            {
                _activeTab = _tabs.Find(t => !t.Disabled);
            }
            StateHasChanged();
            _ = UpdateScrollArrowsAsync();
        }
    }

    private void SelectTab(ThTabPanel tab)
    {
        if (tab.Disabled) return;
        _activeTab = tab;
        StateHasChanged();
    }

    private async Task HandleCloseTab(ThTabPanel tab)
    {
        var args = new TabCloseEventArgs(tab);
        
        if (tab.OnClose.HasDelegate)
        {
            await tab.OnClose.InvokeAsync(args);
        }
        else if (OnTabClose.HasDelegate)
        {
            await OnTabClose.InvokeAsync(args);
        }

        if (!args.Cancel)
        {
            // UnregisterTab will handle active tab change
            _tabs.Remove(tab);
            if (_activeTab == tab)
            {
                _activeTab = _tabs.Find(t => !t.Disabled);
            }
            StateHasChanged();
            _ = UpdateScrollArrowsAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _scrollHandler = await JS.InvokeAsync<IJSObjectReference>("turbohesap.tabs.init", _viewportRef, _leftArrowRef, _rightArrowRef);
            }
            catch
            {
                // Fail silent
            }
        }
    }

    private async Task UpdateScrollArrowsAsync()
    {
        if (_scrollHandler != null)
        {
            try
            {
                await _scrollHandler.InvokeVoidAsync("update");
            }
            catch
            {
                // Fail silent
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_scrollHandler != null)
        {
            try
            {
                await _scrollHandler.InvokeVoidAsync("dispose");
                await _scrollHandler.DisposeAsync();
            }
            catch
            {
                // Fail silent
            }
            _scrollHandler = null;
        }
        GC.SuppressFinalize(this);
    }
}
