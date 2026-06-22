using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Turbohesap.Web.Models;

namespace Turbohesap.Web.Components.Shell;

/// <summary>
/// Kenar çubuğu: rol + arama filtreli menü ağacı. Daraltıldığında alt menüler flyout ile
/// açılır (sidebar.css). Menü tıklaması yalnızca gezinir; sekme açma işini sayfa kendisi
/// üstlenir (ThPage → PageTabService), böylece bulunamayan sayfalar sekme oluşturmaz.
/// </summary>
public partial class Sidebar : IDisposable
{
    [CascadingParameter] private Task<AuthenticationState>? AuthState { get; set; }

    private string _search = string.Empty;
    private readonly HashSet<string> _expanded = [];
    private string[] _roles = [];

    private string SidebarClass =>
        string.Join(' ', new[]
        {
            "th-sidebar",
            Layout.SidebarOpen ? "th-sidebar--open" : null,
            Layout.SidebarCollapsed ? "th-sidebar--collapsed" : null
        }.Where(c => c is not null));

    protected override void OnInitialized()
    {
        Layout.OnChange += StateChanged;
        Nav.LocationChanged += LocationChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (AuthState is not null)
        {
            var state = await AuthState;
            _roles = state.User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();
        }
    }

    private IEnumerable<NavGroup> FilteredGroups()
    {
        foreach (var group in Navigation.Groups)
        {
            var items = group.Items.Select(FilterItem).Where(i => i is not null).Cast<NavItem>().ToList();
            if (items.Count > 0)
            {
                yield return new NavGroup { Title = group.Title, Items = items };
            }
        }
    }

    // Rol + arama filtresi (alt öğeleriyle birlikte).
    private NavItem? FilterItem(NavItem item)
    {
        if (!IsAllowed(item))
        {
            return null;
        }

        var children = item.Children.Select(FilterItem).Where(c => c is not null).Cast<NavItem>().ToList();
        var matches = string.IsNullOrWhiteSpace(_search)
            || item.Label.Contains(_search, StringComparison.OrdinalIgnoreCase)
            || children.Count > 0;

        if (!matches)
        {
            return null;
        }

        if (children.Count > 0 && !string.IsNullOrWhiteSpace(_search))
        {
            _expanded.Add(item.Label);
        }

        return item.HasChildren
            ? new NavItem { Label = item.Label, Icon = item.Icon, Href = item.Href, Roles = item.Roles, Children = children }
            : item;
    }

    private bool IsAllowed(NavItem item)
        => item.Roles.Count == 0 || item.Roles.Any(r => _roles.Contains(r));

    private void OnItemClick(NavItem item)
    {
        if (item.HasChildren)
        {
            if (!_expanded.Add(item.Label))
            {
                _expanded.Remove(item.Label);
            }
            return;
        }

        if (item.Href is not null)
        {
            Go(item.Href);
        }
    }

    // Yalnızca gezinir; sekme açma sayfanın sorumluluğundadır.
    private void Go(string href)
    {
        Layout.CloseSidebar();
        Nav.NavigateTo(href);
    }

    private bool IsActive(string href)
    {
        var path = new Uri(Nav.Uri).AbsolutePath;
        return path.Equals(href, StringComparison.OrdinalIgnoreCase);
    }

    private void OnSearch(ChangeEventArgs e) => _search = e.Value?.ToString() ?? string.Empty;

    private void ClearSearch() => _search = string.Empty;

    private void StateChanged() => InvokeAsync(StateHasChanged);
    private void LocationChanged(object? sender, LocationChangedEventArgs e) => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        Layout.OnChange -= StateChanged;
        Nav.LocationChanged -= LocationChanged;
    }
}
