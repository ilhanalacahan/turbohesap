using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase;

public record City(string Code, string Name);
public record UserItem(string Username, string FullName, string Email, string Role);
public record ProductItem(string Code, string Name, string Price, int Stock, string Icon);

/// <summary>
/// ThAutocomplete bileşeni için dinamik ve interaktif test vitrini.
/// </summary>
public partial class AutocompleteShowcase : ComponentBase
{
    [Inject] private ToastService? ToastService { get; set; }

    // Statik Veriler
    private readonly List<City> _cities = new()
    {
        new("34", "İstanbul"),
        new("06", "Ankara"),
        new("35", "İzmir"),
        new("16", "Bursa"),
        new("07", "Antalya"),
        new("01", "Adana"),
        new("42", "Konya"),
        new("61", "Trabzon")
    };

    private readonly List<UserItem> _users = new()
    {
        new("admin", "Yönetici Kullanıcı", "admin@turbohesap.local", "Admin"),
        new("cihad", "Cihad M.", "cihad@turbohesap.local", "Geliştirici"),
        new("ahmet", "Ahmet Yılmaz", "ahmet@gmail.com", "Müşteri"),
        new("mehmet", "Mehmet Kaya", "mehmet@hotmail.com", "Müşteri Temsilcisi"),
        new("ayse", "Ayşe Demir", "ayse@outlook.com", "Muhasebeci"),
        new("fatma", "Fatma Çelik", "fatma@yahoo.com", "Müşteri")
    };

    private readonly List<ProductItem> _products = new()
    {
        new("P001", "MacBook Pro M3", "84.999 TL", 12, "fa-solid fa-laptop"),
        new("P002", "iPhone 15 Pro", "64.999 TL", 25, "fa-solid fa-mobile-screen"),
        new("P003", "iPad Air", "24.999 TL", 8, "fa-solid fa-tablet-screen-button"),
        new("P004", "AirPods Pro", "8.499 TL", 40, "fa-solid fa-headphones"),
        new("P005", "Apple Watch Series 9", "16.499 TL", 18, "fa-solid fa-clock")
    };

    // Binding Değerleri
    private City? _selectedCity;
    private UserItem? _selectedUser;
    private ProductItem? _selectedProduct;
    private City? _selectedMinLenCity;
    private City? _selectedFocusCity;

    // Asenkron Arama (REST Simülasyonu)
    private async Task<IEnumerable<UserItem>> SearchUsersAsync(string query)
    {
        await Task.Delay(500); // 500 ms yapay ağ gecikmesi

        if (string.IsNullOrWhiteSpace(query)) return _users;

        var q = query.ToLower();
        return _users.Where(u => 
            u.Username.ToLower().Contains(q) || 
            u.FullName.ToLower().Contains(q) || 
            u.Email.ToLower().Contains(q));
    }

    private void OnCitySelected(City city)
    {
        if (city != null)
        {
            ToastService?.Success($"Şehir seçildi: {city.Name} (Plaka: {city.Code})");
        }
    }

    private void OnUserSelected(UserItem user)
    {
        if (user != null)
        {
            ToastService?.Info($"Kullanıcı seçildi: {user.FullName} ({user.Role})");
        }
    }

    private void OnProductSelected(ProductItem product)
    {
        if (product != null)
        {
            ToastService?.Success($"Ürün seçildi: {product.Name} (Fiyat: {product.Price})");
        }
    }
}
