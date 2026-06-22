using Microsoft.JSInterop;

namespace Turbohesap.Web.Services;

/// <summary>window.turbohesap.theme JS API'sine köprü. Tema modu ve token özelleştirme (req 16, 29).</summary>
public sealed class ThemeInterop(IJSRuntime js)
{
    public ValueTask SetModeAsync(string mode) => js.InvokeVoidAsync("turbohesap.theme.setMode", mode);

    public ValueTask<string> ToggleAsync() => js.InvokeAsync<string>("turbohesap.theme.toggle");

    public ValueTask<string> CurrentAsync() => js.InvokeAsync<string>("turbohesap.theme.current");

    public ValueTask<string> GetModeAsync() => js.InvokeAsync<string>("turbohesap.theme.getMode");

    public ValueTask ApplyTokensAsync(IReadOnlyDictionary<string, string> tokens)
        => js.InvokeVoidAsync("turbohesap.theme.applyTokens", tokens);

    public ValueTask ResetTokensAsync() => js.InvokeVoidAsync("turbohesap.theme.resetTokens");

    /// <summary>Yazı tipi ailesini değiştirir (tam CSS font-family zinciri).</summary>
    public ValueTask SetFontAsync(string family) => js.InvokeVoidAsync("turbohesap.theme.setFont", family);

    /// <summary>Yerleşim yoğunluğu: "compact" | "normal" | "comfortable".</summary>
    public ValueTask SetDensityAsync(string density) => js.InvokeVoidAsync("turbohesap.theme.setDensity", density);

    public ValueTask<string> GetDensityAsync() => js.InvokeAsync<string>("turbohesap.theme.getDensity");

    /// <summary>Kart/tablo gölge düzeyi: "none" | "soft" | "medium" | "strong".</summary>
    public ValueTask SetShadowAsync(string level) => js.InvokeVoidAsync("turbohesap.theme.setShadow", level);

    public ValueTask<string> GetShadowAsync() => js.InvokeAsync<string>("turbohesap.theme.getShadow");

    /// <summary>Saklı token override'larını döner (drawer açık durumunu eşitlemek için).</summary>
    public ValueTask<Dictionary<string, string>> GetTokensAsync()
        => js.InvokeAsync<Dictionary<string, string>>("turbohesap.theme.getTokens");
}
