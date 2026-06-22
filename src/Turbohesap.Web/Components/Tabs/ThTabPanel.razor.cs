using Microsoft.AspNetCore.Components;
using System;

namespace Turbohesap.Web.Components.Tabs;

/// <summary>
/// Tek bir sekme paneli (pane) bileşeni.
/// </summary>
public partial class ThTabPanel : ComponentBase, IDisposable
{
    [CascadingParameter] public ThTabs Parent { get; set; } = default!;

    [Parameter] public string Header { get; set; } = string.Empty;
    [Parameter] public string? Icon { get; set; }
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public bool Closable { get; set; } = false;
    [Parameter] public EventCallback<TabCloseEventArgs> OnClose { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public Guid Id { get; } = Guid.NewGuid();

    protected override void OnInitialized()
    {
        if (Parent == null)
        {
            throw new InvalidOperationException("ThTabPanel bileşeni, bir ThTabs bileşeni içinde tanımlanmalıdır.");
        }
        Parent.RegisterTab(this);
    }

    public void Dispose()
    {
        Parent?.UnregisterTab(this);
        GC.SuppressFinalize(this);
    }
}
