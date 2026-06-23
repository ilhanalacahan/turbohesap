using System.Globalization;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Components.Shell;

/// <summary>
/// Tema tasarımcısı drawer'ının kod tarafı. Tüm görsel parametreler (mod, renk, font,
/// yoğunluk, yarıçap, ölçek) <see cref="ThemeInterop"/> üzerinden gerçek zamanlı uygulanır.
/// </summary>
public partial class ThemeDesigner : IDisposable
{
    private sealed record PrimaryPreset(string Name, string Base, string Hover, string Active);
    private sealed record ColorOption(string Label, string Token, string Default);
    private sealed record FontOption(string Label, string Value);

    private string _mode = "system";
    private string _density = "normal";
    private string _shadow = "medium";
    private string _primary = "#2563eb";
    private string _font = "'Inter Variable', system-ui, sans-serif";
    private int _radius = 8;
    private double _scale = 1.0;
    private bool _loaded;

    private readonly (string Value, string Label, string Icon)[] _modes =
    [
        ("light", "Açık", "fa-solid fa-sun"),
        ("dark", "Koyu", "fa-solid fa-moon"),
        ("system", "Sistem", "fa-solid fa-desktop")
    ];

    private readonly (string Value, string Label)[] _densities =
    [
        ("compact", "Kompakt"),
        ("normal", "Normal"),
        ("comfortable", "Rahat")
    ];

    private readonly (string Value, string Label)[] _shadows =
    [
        ("none", "Yok"),
        ("soft", "Hafif"),
        ("medium", "Orta"),
        ("strong", "Belirgin")
    ];

    private readonly PrimaryPreset[] _primaryPresets =
    [
        new("Mavi", "#2563eb", "#1d4ed8", "#1e40af"),
        new("Çivit", "#4f46e5", "#4338ca", "#3730a3"),
        new("Mor", "#7c3aed", "#6d28d9", "#5b21b6"),
        new("Zümrüt", "#059669", "#047857", "#065f46"),
        new("Turkuaz", "#0891b2", "#0e7490", "#155e75"),
        new("Gül", "#e11d48", "#be123c", "#9f1239"),
        new("Amber", "#d97706", "#b45309", "#92400e"),
        new("Antrasit", "#475569", "#334155", "#1e293b")
    ];

    private readonly ColorOption[] _colors =
    [
        new("Vurgu", "--th-accent", "#7c3aed"),
        new("Başarı", "--th-success", "#16a34a"),
        new("Uyarı", "--th-warning", "#d97706"),
        new("Tehlike", "--th-danger", "#dc2626")
    ];

    private readonly FontOption[] _fonts =
    [
        new("Inter", "'Inter Variable', system-ui, sans-serif"),
        new("Manrope", "'Manrope Variable', system-ui, sans-serif"),
        new("Plus Jakarta", "'Plus Jakarta Sans Variable', system-ui, sans-serif"),
        new("IBM Plex Sans", "'IBM Plex Sans', system-ui, sans-serif"),
        new("Sistem", "system-ui, -apple-system, 'Segoe UI', Roboto, sans-serif")
    ];

    protected override void OnInitialized() => Layout.OnChange += Changed;

    // Drawer kapalıdan açığa geçtiğinde geçerli ayarları bir kez yükleyip aktif durumu eşitle;
    // böylece yeniden açınca (veya sayfa yenilenince) panel kaydedilmiş değerleri gösterir.
    private async Task OnLayoutChangedAsync()
    {
        if (Layout.ThemeDesignerOpen && !_loaded)
        {
            _loaded = true;
            await LoadStateAsync();
        }
        else
        {
            if (!Layout.ThemeDesignerOpen)
            {
                _loaded = false;
            }
            StateHasChanged();
        }
    }

    private async Task LoadStateAsync()
    {
        _mode = await Theme.GetModeAsync();
        _density = await Theme.GetDensityAsync();
        _shadow = await Theme.GetShadowAsync();

        var tokens = await Theme.GetTokensAsync();
        if (tokens.TryGetValue("--th-primary", out var primary))
        {
            _primary = primary;
        }
        if (tokens.TryGetValue("--th-font-sans", out var font))
        {
            _font = font;
        }
        if (tokens.TryGetValue("--th-radius-base", out var radius) &&
            int.TryParse(radius.Replace("px", string.Empty).Trim(), out var radiusPx))
        {
            _radius = radiusPx;
        }
        if (tokens.TryGetValue("--th-scale", out var scale) &&
            double.TryParse(scale, NumberStyles.Float, CultureInfo.InvariantCulture, out var scaleValue))
        {
            _scale = scaleValue;
        }

        StateHasChanged();
    }

    private static string SegmentClass(bool active) => active ? "th-segment__btn th-segment__btn--active" : "th-segment__btn";
    private static string SwatchClass(bool active) => active ? "th-swatch th-swatch--active" : "th-swatch";
    private static string FontClass(bool active) => active ? "th-tds-font th-tds-font--active" : "th-tds-font";

    private async Task SetModeAsync(string mode)
    {
        _mode = mode;
        await Theme.SetModeAsync(mode);
    }

    private async Task SetDensityAsync(string density)
    {
        _density = density;
        await Theme.SetDensityAsync(density);
    }

    private async Task SetPrimaryAsync(PrimaryPreset preset)
    {
        _primary = preset.Base;
        await Theme.ApplyTokensAsync(new Dictionary<string, string>
        {
            ["--th-primary"] = preset.Base,
            ["--th-primary-hover"] = preset.Hover,
            ["--th-primary-active"] = preset.Active
        });
    }

    private async Task SetCustomPrimaryAsync(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        _primary = value;
        await Theme.ApplyTokensAsync(new Dictionary<string, string> { ["--th-primary"] = value });
    }

    private async Task SetTokenAsync(string token, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            await Theme.ApplyTokensAsync(new Dictionary<string, string> { [token] = value });
        }
    }

    private async Task SetFontAsync(string family)
    {
        _font = family;
        await Theme.SetFontAsync(family);
    }

    private async Task SetShadowAsync(string level)
    {
        _shadow = level;
        await Theme.SetShadowAsync(level);
    }

    private async Task OnRadius(ChangeEventArgs e)
    {
        // Tek taban; tüm yarıçaplar (kart, tablo, dialog, buton...) bundan türer.
        if (int.TryParse(e.Value?.ToString(), out _radius))
        {
            await Theme.ApplyTokensAsync(new Dictionary<string, string> { ["--th-radius-base"] = $"{_radius}px" });
        }
    }

    private async Task OnScale(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), CultureInfo.InvariantCulture, out _scale))
        {
            await Theme.ApplyTokensAsync(new Dictionary<string, string>
            {
                ["--th-scale"] = _scale.ToString("0.00", CultureInfo.InvariantCulture)
            });
        }
    }

    private async Task ResetAsync()
    {
        _radius = 8;
        _scale = 1.0;
        _primary = "#2563eb";
        _font = _fonts[0].Value;
        _density = "normal";
        _shadow = "medium";
        await Theme.ResetTokensAsync();
        Toasts.Info("Tema varsayılana döndürüldü.");
    }

    private void Changed() => InvokeAsync(OnLayoutChangedAsync);

    public void Dispose() => Layout.OnChange -= Changed;
}
