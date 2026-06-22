using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Turbohesap.Web.Models;

namespace Turbohesap.Web.Components.Shell;

/// <summary>
/// PageTabs şeridinin kod tarafı. Kirli bir sekme kapatılmak istendiğinde önce onay diyaloğu
/// gösterilir; onaylanırsa sekme kapatılır ve komşu sekmeye gidilir.
/// </summary>
public partial class PageTabs : IDisposable
{
    private bool _confirmOpen;
    private PageTab? _pending;

    protected override void OnInitialized()
    {
        Tabs.OnChange += Changed;
        Nav.LocationChanged += OnLocationChanged;
        SyncActive();
    }

    private bool IsActive(PageTab tab) => string.Equals(Tabs.ActiveHref, tab.Href, StringComparison.OrdinalIgnoreCase);

    private void Activate(PageTab tab)
    {
        Tabs.SetActive(tab.Href);
        Nav.NavigateTo(tab.Href);
    }

    private void RequestClose(PageTab tab)
    {
        if (tab.IsDirty)
        {
            _pending = tab;
            _confirmOpen = true;
            return;
        }
        CloseTab(tab.Href);
    }

    private void ConfirmClose()
    {
        if (_pending is not null)
        {
            CloseTab(_pending.Href);
            _pending = null;
        }
        _confirmOpen = false;
    }

    private void CloseTab(string href)
    {
        var next = Tabs.Close(href);
        if (next is not null)
        {
            Nav.NavigateTo(next);
        }
    }

    private void SyncActive()
    {
        var path = new Uri(Nav.Uri).AbsolutePath;
        if (Tabs.Tabs.Any(t => string.Equals(t.Href, path, StringComparison.OrdinalIgnoreCase)))
        {
            Tabs.SetActive(path);
        }
    }

    private void Changed() => InvokeAsync(StateHasChanged);

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        SyncActive();
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Tabs.OnChange -= Changed;
        Nav.LocationChanged -= OnLocationChanged;
    }
}
