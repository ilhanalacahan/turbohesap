using System.Timers;
using Microsoft.AspNetCore.Components;
using Turbohesap.Shared.Contracts.Auth;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Auth;

/// <summary>
/// Kurumsal split giriş sayfası: solda otomatik dönen tanıtım slaytları, sağda giriş formu.
/// </summary>
public partial class Login : IDisposable
{
    private sealed record Slide(string Icon, string Title, string Text);

    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }

    private readonly LoginRequest _model = new();
    private bool _busy;
    private int _slide;
    private System.Timers.Timer? _timer;

    private readonly Slide[] _slides =
    [
        new("fa-solid fa-bolt", "Hızlı İşlem", "Fatura, tahsilat ve cari hareketleri saniyeler içinde kaydedin."),
        new("fa-solid fa-users-gear", "Müşteri & CRM", "Cari takibi, görüşme notları ve satış fırsatlarını tek yerden yönetin."),
        new("fa-solid fa-industry", "Esnek Üretim", "İş emirleri, ürün reçeteleri ve istasyon durumlarını anlık izleyin."),
        new("fa-solid fa-calendar-check", "Akıllı Planlama", "Makine yükleri, üretim takvimleri ve gantt süreç takibi."),
        new("fa-solid fa-shield-halved", "Güvenli Altyapı", "JWT tabanlı kimlik, denetim kayıtları ve uçtan uca şifreleme."),
        new("fa-solid fa-chart-line", "Anlık Raporlar", "Satış özeti ve cari bakiyeleri gerçek zamanlı izleyin.")
    ];

    protected override void OnInitialized()
    {
        _timer = new System.Timers.Timer(5000);
        _timer.Elapsed += NextSlide;
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void NextSlide(object? sender, ElapsedEventArgs e)
    {
        _slide = (_slide + 1) % _slides.Length;
        InvokeAsync(StateHasChanged);
    }

    private void ShowSlide(int index)
    {
        _slide = index;
        _timer?.Stop();
        _timer?.Start();
    }

    private async Task SubmitAsync()
    {
        _busy = true;
        try
        {
            await Auth.LoginAsync(_model);
            var target = string.IsNullOrEmpty(ReturnUrl) ? "/dashboard" : Uri.UnescapeDataString(ReturnUrl);
            Nav.NavigateTo(target, forceLoad: false);
        }
        catch (ApiException ex)
        {
            Toasts.Error(ex.Message, "Giriş başarısız");
        }
        finally
        {
            _busy = false;
        }
    }

    public void Dispose()
    {
        if (_timer is not null)
        {
            _timer.Elapsed -= NextSlide;
            _timer.Dispose();
        }
    }
}
