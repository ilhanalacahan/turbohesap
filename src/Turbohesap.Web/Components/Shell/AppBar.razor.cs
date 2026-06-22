using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace Turbohesap.Web.Components.Shell;

/// <summary>Üst bar: breadcrumb, komut araması tetikleyicisi, tema/bildirim/gece-gündüz, kullanıcı menüsü.</summary>
public partial class AppBar : IDisposable
{
    [CascadingParameter] private Task<AuthenticationState>? AuthState { get; set; }

    private List<string> _crumbs = ["Gösterge Paneli"];
    private bool _userMenuOpen;
    private string _initials = "TH";
    private string _fullName = "Kullanıcı";
    private string _email = string.Empty;

    protected override void OnInitialized()
    {
        Nav.LocationChanged += OnLocationChanged;
        BuildCrumbs();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (AuthState is not null)
        {
            var user = (await AuthState).User;
            _fullName = user.Identity?.Name ?? "Kullanıcı";
            _email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;
            _initials = BuildInitials(_fullName);
        }
    }

    private void BuildCrumbs()
    {
        var path = new Uri(Nav.Uri).AbsolutePath.Trim('/');
        _crumbs = string.IsNullOrEmpty(path)
            ? ["Gösterge Paneli"]
            : path.Split('/').Select(Humanize).ToList();
    }

    private static string Humanize(string segment)
        => segment.Length == 0 ? segment : char.ToUpperInvariant(segment[0]) + segment[1..].Replace('-', ' ');

    private static string BuildInitials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 0 ? "TH" : string.Concat(parts.Take(2).Select(p => char.ToUpperInvariant(p[0])));
    }

    private void ToggleUserMenu() => _userMenuOpen = !_userMenuOpen;

    private async Task ToggleDarkAsync() => await Theme.ToggleAsync();

    private async Task LogoutAsync()
    {
        _userMenuOpen = false;
        await Auth.LogoutAsync();
        Nav.NavigateTo("/login", forceLoad: false);
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        BuildCrumbs();
        _userMenuOpen = false;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose() => Nav.LocationChanged -= OnLocationChanged;
}
