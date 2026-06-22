using Turbohesap.Shared.Security;
using Turbohesap.Web.Models;

namespace Turbohesap.Web.Services;

/// <summary>
/// Sidebar menü ağacı ve uygulama başlatıcı içeriğinin tek tanımı. Menü öğeleri rol
/// bilgisi taşır; sidebar yalnızca kullanıcının yetkili olduğu öğeleri gösterir.
/// Sayfa yolları API gibidir ama /api/{version} öneki yoktur (req 24).
/// </summary>
public sealed class AppNavigation
{
    public IReadOnlyList<NavGroup> Groups { get; } =
    [
        new NavGroup
        {
            Title = "Genel",
            Items =
            [
                new NavItem { Label = "Gösterge Paneli", Icon = "fa-solid fa-gauge-high", Href = "/dashboard" }
            ]
        },
        new NavGroup
        {
            Title = "Cari",
            Items =
            [
                new NavItem
                {
                    Label = "Müşteriler",
                    Icon = "fa-solid fa-users",
                    Href = "/customers",
                    Roles = [Roles.Administrator, Roles.Manager, Roles.Accountant]
                },
                new NavItem
                {
                    Label = "Raporlar",
                    Icon = "fa-solid fa-chart-line",
                    Roles = [Roles.Administrator, Roles.Manager],
                    Children =
                    [
                        new NavItem { Label = "Satış Özeti", Icon = "fa-solid fa-receipt", Href = "/reports/sales", Roles = [Roles.Administrator, Roles.Manager] },
                        new NavItem { Label = "Cari Bakiye", Icon = "fa-solid fa-scale-balanced", Href = "/reports/balance", Roles = [Roles.Administrator, Roles.Manager] }
                    ]
                }
            ]
        },
        new NavGroup
        {
            Title = "Sistem",
            Items =
            [
                new NavItem { Label = "Denetim Kayıtları", Icon = "fa-solid fa-clipboard-list", Href = "/audit-logs", Roles = [Roles.Administrator] },
                new NavItem { Label = "Hata Kayıtları", Icon = "fa-solid fa-bug", Href = "/error-logs", Roles = [Roles.Administrator] },
                new NavItem { Label = "Ayarlar", Icon = "fa-solid fa-gear", Href = "/settings" }
            ]
        }
    ];

    public IReadOnlyList<AppItem> Apps { get; } =
    [
        new AppItem { Name = "Turbohesap", Icon = "fa-solid fa-calculator", Href = "/dashboard", Description = "Ön muhasebe ve cari", Accent = "#2563eb" },
        new AppItem { Name = "CRM", Icon = "fa-solid fa-handshake", Href = "#", Description = "Müşteri ilişkileri", Accent = "#7c3aed" },
        new AppItem { Name = "Stok", Icon = "fa-solid fa-boxes-stacked", Href = "#", Description = "Depo ve envanter", Accent = "#0891b2" },
        new AppItem { Name = "İK", Icon = "fa-solid fa-id-badge", Href = "#", Description = "İnsan kaynakları", Accent = "#16a34a" },
        new AppItem { Name = "Belgeler", Icon = "fa-solid fa-folder-open", Href = "#", Description = "Doküman yönetimi", Accent = "#d97706" },
        new AppItem { Name = "Takvim", Icon = "fa-solid fa-calendar-days", Href = "#", Description = "Planlama ve ajanda", Accent = "#e11d48" }
    ];
}
