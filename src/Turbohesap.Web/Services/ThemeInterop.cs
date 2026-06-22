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
}
