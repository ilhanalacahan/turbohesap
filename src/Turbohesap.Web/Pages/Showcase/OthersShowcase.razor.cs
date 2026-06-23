using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Turbohesap.Web.Components.Breadcrumb;

namespace Turbohesap.Web.Pages.Showcase;

/// <summary>
/// Tooltip, Popover, Alert, Breadcrumb ve Spinner bileşenleri için test vitrini.
/// </summary>
public partial class OthersShowcase : ComponentBase
{
    [Inject] private Services.ToastService ToastService { get; set; } = default!;

    private readonly List<BreadcrumbItem> _breadcrumbItems = new()
    {
        new() { Text = "Ana Sayfa", Url = "/showcase", Icon = "fa-solid fa-house" },
        new() { Text = "Ayarlar", Url = "/showcase", Icon = "fa-solid fa-gear" },
        new() { Text = "Kullanıcı Profili", Icon = "fa-solid fa-user" }
    };

    private bool _showGrid = true;
    private bool _enableLogs = false;

    private void OnAlertClosed()
    {
        // OnClose callback tetiklendiğinde burası çalışır.
    }

    private void OnEmptyStateActionClick()
    {
        ToastService.Success("Yeni fatura oluşturma ekranı tetiklendi.", "Fatura Oluştur");
    }
}
