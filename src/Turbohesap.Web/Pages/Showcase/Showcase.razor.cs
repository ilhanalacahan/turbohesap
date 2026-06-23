using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Pages.Showcase;

public enum ShowcaseTab
{
    Inputs,
    Autocomplete,
    ListView,
    DataTable,
    Dialogs,
    Navigation,
    Display
}

/// <summary>
/// Showcase sayfasının C# katmanı. Bileşen vitrini sekmeli ve lazy-load yükleme desteğiyle render edilir.
/// </summary>
public partial class Showcase : ComponentBase
{
    private ShowcaseTab _activeTab = ShowcaseTab.Inputs;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        // İlk yüklemede 250 ms sahte bekleme
        _isLoading = true;
        await Task.Delay(250);
        _isLoading = false;
    }

    private async Task SetTabAsync(ShowcaseTab tab)
    {
        if (_activeTab == tab) return;

        _isLoading = true;
        _activeTab = tab;
        StateHasChanged(); // Spinner'ın hemen görünmesi için tetikle

        // Sahte 250 ms lazy load bekleme
        await Task.Delay(250);

        _isLoading = false;
        StateHasChanged();
    }

    private string GetTabStatistics() => _activeTab switch
    {
        ShowcaseTab.Inputs => "BİLEŞEN SAYISI: 9 | FOCUS TRAP: AKTİF | DOĞRULAMA: ENTEGRE",
        ShowcaseTab.Autocomplete => "BİLEŞEN SAYISI: 1 | REST DESTEĞİ: AKTİF | DEBOUNCE: 300ms",
        ShowcaseTab.ListView => "BİLEŞEN SAYISI: 1 | SONSUZ KAYDIRMA: AKTİF | YÜKLEME TAMPONU: AKTİF",
        ShowcaseTab.DataTable => "BİLEŞEN SAYISI: 3 | BİLEŞEN TİPİ: GENERIC | SIRALAMA & SAYFALAMA: ENTEGRE",
        ShowcaseTab.Dialogs => "BİLEŞEN SAYISI: 4 | KATMAN YÖNETİMİ: Z-INDEX PORTAL | ANİMASYON GEÇİŞİ: 150ms",
        ShowcaseTab.Navigation => "BİLEŞEN SAYISI: 3 | SEKME NAVİGASYONU: AKTİF | AKORDİYON GRUPLARI: ENTEGRE",
        ShowcaseTab.Display => "BİLEŞEN SAYISI: 9 | GÖRSEL TOKEN UYUMU: %100 | GRAFİK KADRANI: SVG TABANLI",
        _ => ""
    };
}
