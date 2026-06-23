using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Binlik ayraç destekli ve entegre hesap makinesi modalına sahip para birimi giriş bileşeni.
/// </summary>
public partial class ThCurrencyInput : TurboComponentBase, IAsyncDisposable
{
    private static readonly CultureInfo TrCulture = new CultureInfo("tr-TR");

    [Inject] private IJSRuntime Js { get; set; } = default!;

    [Parameter] public decimal? Value { get; set; }
    [Parameter] public EventCallback<decimal?> ValueChanged { get; set; }
    
    [Parameter] public string Label { get; set; } = "";
    [Parameter] public string Placeholder { get; set; } = "0,00";
    [Parameter] public string CurrencySymbol { get; set; } = "₺";
    [Parameter] public bool ShowCalculator { get; set; } = true;
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string _displayText = "";
    private bool _isCalculatorOpen = false;
    private decimal? _lastValue;
    
    private ElementReference _inputRef;
    private DotNetObjectReference<ThCurrencyInput>? _dotNetRef;
    private bool _jsInitialized = false;

    protected override async Task OnParametersSetAsync()
    {
        if (Value != _lastValue)
        {
            _lastValue = Value;
            await FormatDisplayValueAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await Js.InvokeVoidAsync("window.turbohesap.initCurrencyInput", _inputRef, _dotNetRef);
            _jsInitialized = true;
            await FormatDisplayValueAsync();
        }
    }

    private string RootClass => Cx(
        "th-currency-input-wrapper",
        SizeClass(Size, "th-currency-input-wrapper"),
        Class);

    private async Task FormatDisplayValueAsync()
    {
        if (Value.HasValue)
        {
            _displayText = Value.Value.ToString("N2", TrCulture);
        }
        else
        {
            _displayText = "";
        }

        if (_jsInitialized)
        {
            await Js.InvokeVoidAsync("window.turbohesap.setCurrencyInputValue", _inputRef, _displayText);
        }
    }

    [JSInvokable]
    public async Task OnCurrencyInputChanged(double rawVal, string formattedValue)
    {
        decimal? calculated = (decimal)rawVal;
        if (calculated == 0 && string.IsNullOrEmpty(formattedValue))
        {
            calculated = null;
        }

        _displayText = formattedValue;
        _lastValue = calculated;

        if (Value != calculated)
        {
            Value = calculated;
            await ValueChanged.InvokeAsync(calculated);
        }
    }

    private void OpenCalculator()
    {
        if (Disabled) return;
        _isCalculatorOpen = true;
    }

    private void CloseCalculator()
    {
        _isCalculatorOpen = false;
    }

    private async Task HandleCalculatorValue(decimal calculatedValue)
    {
        Value = calculatedValue;
        _lastValue = calculatedValue;
        await ValueChanged.InvokeAsync(calculatedValue);
        await FormatDisplayValueAsync();
        _isCalculatorOpen = false;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsInitialized && _inputRef.Id != null)
            {
                await Js.InvokeVoidAsync("window.turbohesap.disposeCurrencyInput", _inputRef);
            }
        }
        catch (JSDisconnectedException)
        {
            // Bağlantı koptuysa yoksay
        }
        finally
        {
            _dotNetRef?.Dispose();
        }
    }
}
