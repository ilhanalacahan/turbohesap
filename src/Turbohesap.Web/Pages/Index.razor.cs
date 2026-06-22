namespace Turbohesap.Web.Pages;

/// <summary>Kök sayfa: oturum varsa /dashboard, yoksa /login'e yönlendirir.</summary>
public partial class Index
{
    protected override async Task OnInitializedAsync()
    {
        var state = await AuthProvider.GetAuthenticationStateAsync();
        var authenticated = state.User.Identity?.IsAuthenticated == true;
        Nav.NavigateTo(authenticated ? "/dashboard" : "/login", forceLoad: false);
    }
}
