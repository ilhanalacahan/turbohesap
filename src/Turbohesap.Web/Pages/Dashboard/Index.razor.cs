namespace Turbohesap.Web.Pages.Dashboard;

/// <summary>Gösterge paneli özet kartları (demo veriler).</summary>
public partial class Index
{
    private sealed record Stat(string Label, string Value, string Icon, string IconBg, string IconFg);

    private readonly Stat[] _stats =
    [
        new("Toplam Müşteri", "128", "fa-solid fa-users", "bg-primary-subtle", "text-primary"),
        new("Aylık Satış", "₺ 84.300", "fa-solid fa-arrow-trend-up", "bg-success-subtle", "text-success"),
        new("Bekleyen Fatura", "12", "fa-solid fa-file-invoice", "bg-warning-subtle", "text-warning"),
        new("Açık Talep", "5", "fa-solid fa-headset", "bg-info-subtle", "text-info")
    ];
}
