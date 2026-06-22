using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>
/// Standart <see cref="ErrorBoundary"/>'nin üzerine, yakalanan istemci hatasını API'ye
/// (error_logs) raporlayan sürüm. Alt bileşenlerin yaşam döngüsü/olay işleyici hatalarını
/// yakalar; rapor sonrası <c>ErrorContent</c> yedek arayüzü gösterilir.
/// </summary>
public sealed class ThErrorBoundary : ErrorBoundary
{
    [Inject] private WebErrorReporter Reporter { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected override async Task OnErrorAsync(Exception exception)
    {
        await Reporter.ReportAsync(exception, new Uri(Nav.Uri).PathAndQuery);
        await base.OnErrorAsync(exception);
    }
}
