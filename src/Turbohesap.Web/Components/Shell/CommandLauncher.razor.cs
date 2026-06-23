using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Turbohesap.Web.Models;

namespace Turbohesap.Web.Components.Shell;

/// <summary>Komut paleti: sayfa arama + gezinme. Sekme açma sayfanın (ThPage) sorumluluğundadır.</summary>
public partial class CommandLauncher : IDisposable
{
    private string _search = string.Empty;

    protected override void OnInitialized() => Layout.OnChange += Changed;

    private static IEnumerable<NavItem> Flatten(IEnumerable<NavItem> items)
    {
        foreach (var item in items)
        {
            if (item.Href is not null)
            {
                yield return item;
            }
            foreach (var child in Flatten(item.Children))
            {
                yield return child;
            }
        }
    }

    private IEnumerable<NavItem> Results()
    {
        var all = Navigation.Groups.SelectMany(g => Flatten(g.Items));
        return string.IsNullOrWhiteSpace(_search)
            ? all.Take(8)
            : all.Where(i => i.Label.Contains(_search, StringComparison.OrdinalIgnoreCase)).Take(12);
    }

    private void Go(NavItem item)
    {
        if (item.Href is null)
        {
            return;
        }
        // Sekme açma sayfanın sorumluluğundadır (ThPage). Burada yalnızca gezinilir.
        Layout.CloseCommand();
        Nav.NavigateTo(item.Href);
    }

    private void OnSearch(ChangeEventArgs e) => _search = e.Value?.ToString() ?? string.Empty;

    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            Layout.CloseCommand();
        }
        else if (e.Key == "Enter")
        {
            var first = Results().FirstOrDefault();
            if (first is not null)
            {
                Go(first);
            }
        }
    }

    private void Changed() => InvokeAsync(StateHasChanged);

    public void Dispose() => Layout.OnChange -= Changed;
}
