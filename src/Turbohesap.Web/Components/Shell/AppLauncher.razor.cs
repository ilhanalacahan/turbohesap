using Turbohesap.Web.Models;

namespace Turbohesap.Web.Components.Shell;

/// <summary>Uygulama başlatıcının kod tarafı. Aktif olmayan ("#") uygulamalar gezinmez.</summary>
public partial class AppLauncher : IDisposable
{
    protected override void OnInitialized() => Layout.OnChange += Changed;

    private void Open(AppItem app)
    {
        if (app.ComingSoon)
        {
            return;
        }
        Layout.CloseAppLauncher();
        Nav.NavigateTo(app.Href);
    }

    private void Changed() => InvokeAsync(StateHasChanged);

    public void Dispose() => Layout.OnChange -= Changed;
}
