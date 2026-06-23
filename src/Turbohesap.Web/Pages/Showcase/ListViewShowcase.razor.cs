using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase;

public record CountryItem(string Code, string Name, string Icon, string Continent);
public record ShoppingItem(string Id, string Name, string Category, string Price);
public record LogEntry(string Timestamp, string Message, string Level, string Source);

/// <summary>
/// ThListView bileşeni için dinamik ve interaktif test vitrini.
/// </summary>
public partial class ListViewShowcase : ComponentBase
{
    [Inject] private ToastService? ToastService { get; set; }

    // Statik Veriler (Senaryo 1)
    private readonly List<CountryItem> _countries = new()
    {
        new("TR", "Türkiye", "fa-solid fa-flag", "Asya/Avrupa"),
        new("DE", "Almanya", "fa-solid fa-flag", "Avrupa"),
        new("US", "Amerika Birleşik Devletleri", "fa-solid fa-flag", "Kuzey Amerika"),
        new("GB", "Birleşik Krallık", "fa-solid fa-flag", "Avrupa"),
        new("FR", "Fransa", "fa-solid fa-flag", "Avrupa"),
        new("IT", "İtalya", "fa-solid fa-flag", "Avrupa"),
        new("JP", "Japonya", "fa-solid fa-flag", "Asya"),
        new("CN", "Çin", "fa-solid fa-flag", "Asya")
    };

    // Statik Veriler (Senaryo 2)
    private readonly List<ShoppingItem> _shoppingList = new()
    {
        new("1", "Süt 1L", "Gıda", "24.50 TL"),
        new("2", "Ekmek", "Gıda", "8.00 TL"),
        new("3", "Kahve 250g", "Gıda", "120.00 TL"),
        new("4", "Deterjan 5kg", "Temizlik", "185.00 TL"),
        new("5", "Şampuan", "Kişisel Bakım", "75.00 TL"),
        new("6", "Diş Macunu", "Kişisel Bakım", "45.00 TL")
    };

    // Dinamik Veriler (Sonsuz Kaydırma - Senaryo 3)
    private List<LogEntry> _logs = new();
    private bool _hasMoreLogs = true;
    private int _logPage = 1;

    // Seçim Durumları
    private CountryItem? _selectedCountry;
    private List<ShoppingItem> _selectedShoppingItems = new();

    protected override void OnInitialized()
    {
        LoadMoreLogs();
    }

    private void LoadMoreLogs()
    {
        var start = (_logPage - 1) * 10;
        for (int i = 1; i <= 10; i++)
        {
            var idx = start + i;
            var level = idx % 3 == 0 ? "danger" : (idx % 5 == 0 ? "warning" : "info");
            _logs.Add(new LogEntry(
                DateTime.Now.AddSeconds(-idx * 5).ToString("HH:mm:ss"),
                $"Uygulama log kaydı #{idx} - Sistem durumu kontrol ediliyor.",
                level,
                "AuthService"
            ));
        }

        _logPage++;
        if (_logPage > 5)
        {
            _hasMoreLogs = false;
        }
    }

    private async Task OnLoadMoreLogsAsync()
    {
        await Task.Delay(800); // 800 ms yapay ağ/servis gecikmesi
        LoadMoreLogs();
        ToastService?.Info("Yeni log verileri yüklendi.");
    }

    private void OnCountryChanged(CountryItem? country)
    {
        _selectedCountry = country;
        if (country != null)
        {
            ToastService?.Success($"Seçilen Ülke: {country.Name} ({country.Continent})");
        }
        else
        {
            ToastService?.Info("Ülke seçimi kaldırıldı.");
        }
    }

    private void OnShoppingSelectionChanged(List<ShoppingItem> items)
    {
        _selectedShoppingItems = items;
        ToastService?.Success($"Seçili Ürün Sayısı: {items.Count}");
    }
}
