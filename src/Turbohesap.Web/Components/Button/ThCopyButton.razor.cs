using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Button;

/// <summary>
/// Metin değerlerini tek tıkla panoya kopyalayan ve animasyonlu onay dönütü veren mini kopyalama butonu.
/// </summary>
public partial class ThCopyButton : TurboComponentBase
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public string Text { get; set; } = string.Empty;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Sm;
    [Parameter] public string TooltipText { get; set; } = "Kopyala";
    [Parameter] public string SuccessTooltipText { get; set; } = "Kopyalandı!";

    private bool _copied = false;

    private string RootClass => Cx(
        "th-copy-button",
        _copied ? "th-copy-button--copied" : "",
        SizeClass(Size, "th-copy-button"),
        Class);

    private async Task CopyToClipboard()
    {
        if (string.IsNullOrEmpty(Text) || _copied) return;

        try
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", Text);
            _copied = true;
            StateHasChanged();
            
            // 2 saniye sonra kopyalandı ikonunu eski haline döndür
            await Task.Delay(2000);
            _copied = false;
            StateHasChanged();
        }
        catch
        {
            // Fail silent
        }
    }
}
