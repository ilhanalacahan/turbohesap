using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Telefon, TC Kimlik No veya özel kalıplar için dinamik maskeleme desteği sunan giriş kutusu.
/// </summary>
public partial class ThMaskInput : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime Js { get; set; } = default!;

    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    [Parameter] public string Label { get; set; } = "";
    [Parameter] public string Placeholder { get; set; } = "";
    [Parameter] public string Mask { get; set; } = "0(###) ### ## ##"; // Örnek: "0(###) ### ## ##" veya "###-###-####"
    [Parameter] public bool ReturnRawValue { get; set; } = true; // true ise "03465121017", false ise "0(346) 512 10 17"
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    /// <summary>Sol tarafa eklenecek buton vb. bileşenler.</summary>
    [Parameter] public RenderFragment? Prepend { get; set; }

    /// <summary>Sağ tarafa eklenecek buton vb. bileşenler.</summary>
    [Parameter] public RenderFragment? Append { get; set; }

    private ElementReference _inputRef;
    private DotNetObjectReference<ThMaskInput>? _dotNetRef;
    private bool _jsInitialized = false;
    private string? _lastValue;

    protected override async Task OnParametersSetAsync()
    {
        if (Value != _lastValue)
        {
            _lastValue = Value;
            if (_jsInitialized && Value != null)
            {
                await Js.InvokeVoidAsync("window.turbohesap.setMaskInputValue", _inputRef, Value);
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await Js.InvokeVoidAsync("window.turbohesap.initMaskInput", _inputRef, Mask, _dotNetRef);
            _jsInitialized = true;
            if (Value != null)
            {
                await Js.InvokeVoidAsync("window.turbohesap.setMaskInputValue", _inputRef, Value);
            }
        }
    }

    private string RootClass => Cx(
        "th-mask-input-wrapper",
        SizeClass(Size, "th-mask-input-wrapper"),
        Class);

    [JSInvokable]
    public async Task OnMaskedInputChanged(string formattedValue, string rawValue)
    {
        var targetValue = ReturnRawValue ? rawValue : formattedValue;
        if (Value != targetValue)
        {
            Value = targetValue;
            _lastValue = targetValue;
            await ValueChanged.InvokeAsync(targetValue);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsInitialized && _inputRef.Id != null)
            {
                await Js.InvokeVoidAsync("window.turbohesap.disposeMaskInput", _inputRef);
            }
        }
        catch (JSDisconnectedException)
        {
            // Bağlantı kesildiyse yok say
        }
        finally
        {
            _dotNetRef?.Dispose();
        }
    }
}
