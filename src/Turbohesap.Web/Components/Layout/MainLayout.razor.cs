using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Turbohesap.Web.Components.Feedback;

namespace Turbohesap.Web.Components.Layout;

/// <summary>
/// Kimliği doğrulanmış kabuk (sidebar + appbar + sekmeler + içerik). İçerik bir
/// <see cref="ThErrorBoundary"/> ile sarılır; yakalanan hatalar error_logs'a raporlanır ve
/// gezinince sınır sıfırlanır.
/// </summary>
public partial class MainLayout : IDisposable
{
    private bool _ready;
    private ThErrorBoundary? _errorBoundary;

    protected override async Task OnInitializedAsync()
    {
        Layout.OnChange += Changed;
        AuthProvider.AuthenticationStateChanged += OnAuthChanged;
        Nav.LocationChanged += RecoverError;

        var state = await AuthProvider.GetAuthenticationStateAsync();
        if (state.User.Identity?.IsAuthenticated == true)
        {
            _ready = true;
        }
        else
        {
            RedirectToLogin();
        }
    }

    private void RedirectToLogin()
    {
        var returnUrl = Uri.EscapeDataString(new Uri(Nav.Uri).PathAndQuery);
        Nav.NavigateTo($"/login?returnUrl={returnUrl}", forceLoad: false);
    }

    private async void OnAuthChanged(Task<AuthenticationState> task)
    {
        var state = await task;
        if (state.User.Identity?.IsAuthenticated != true)
        {
            _ready = false;
            await InvokeAsync(RedirectToLogin);
        }
    }

    private void Changed() => InvokeAsync(StateHasChanged);

    // Gezinince hata sınırını sıfırla; yeni sayfa temiz başlasın.
    private void RecoverError(object? sender, LocationChangedEventArgs e) => _errorBoundary?.Recover();

    public void Dispose()
    {
        Layout.OnChange -= Changed;
        AuthProvider.AuthenticationStateChanged -= OnAuthChanged;
        Nav.LocationChanged -= RecoverError;
    }
}
