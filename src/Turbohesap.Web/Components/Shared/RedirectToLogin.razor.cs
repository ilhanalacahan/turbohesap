namespace Turbohesap.Web.Components.Shared;

/// <summary>Yetkisiz erişimde /login'e yönlendirir; dönüş adresini korur.</summary>
public partial class RedirectToLogin
{
    protected override void OnInitialized()
    {
        var returnUrl = Uri.EscapeDataString(new Uri(Nav.Uri).PathAndQuery);
        Nav.NavigateTo($"/login?returnUrl={returnUrl}", forceLoad: false);
    }
}
