using Turbohesap.Shared.Contracts.Diagnostics;

namespace Turbohesap.Web.Services;

/// <summary>
/// Web (Blazor) tarafında yakalanan hataları API'ye gönderir; API bunları sunucu hatalarıyla
/// aynı <c>error_logs</c> tablosuna (Source = "Web") işler. Raporlama oturum gerektirmez ve
/// kendi hatalarını yutar (raporlama başarısız olsa bile uygulamayı düşürmez).
/// </summary>
public sealed class WebErrorReporter(ApiClient api, ILogger<WebErrorReporter> logger)
{
    private const string Endpoint = "api/v1/diagnostics/client-errors";

    public async Task ReportAsync(Exception exception, string? path)
    {
        try
        {
            await api.PostAsync<bool>(Endpoint, new WebErrorReport
            {
                Message = exception.Message,
                ExceptionType = exception.GetType().FullName ?? exception.GetType().Name,
                StackTrace = exception.StackTrace,
                Source = exception.Source,
                Path = path
            });
        }
        catch (Exception reportEx)
        {
            // Hata raporu gönderilemese bile kullanıcı akışını bozmuyoruz.
            logger.LogWarning(reportEx, "İstemci hatası API'ye raporlanamadı.");
        }
    }
}
