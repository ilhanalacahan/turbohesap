using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Ticari işlemler için KDV butonlu, modern ve estetik hesap makinesi bileşeni.
/// </summary>
public partial class ThCalculator : TurboComponentBase
{
    private static readonly CultureInfo TrCulture = new CultureInfo("tr-TR");

    [Inject] private IJSRuntime Js { get; set; } = default!;

    [Parameter] public decimal InitialValue { get; set; }
    [Parameter] public EventCallback<decimal> OnValueSelected { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    private string _expression = "";
    private string _result = "0";
    private bool _hasError = false;
    private ElementReference _calculatorRef;

    protected override void OnInitialized()
    {
        if (InitialValue != 0)
        {
            _expression = InitialValue.ToString("0.##", CultureInfo.InvariantCulture);
            _result = InitialValue.ToString("N2", TrCulture);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await Js.InvokeVoidAsync("window.turbohesap.focus", _calculatorRef);
            }
            catch
            {
                // JS ortamı hazır değilse sessizce geç
            }
        }
    }

    private string RootClass => Cx(
        "th-calculator",
        Class);

    private async Task AppendCharacter(string character)
    {
        if (_hasError) Clear();

        // Operatörlerin ardışık gelmesini engelle
        if (IsOperator(character) && _expression.Length > 0 && IsOperator(_expression[^1..]))
        {
            _expression = _expression[..^1] + character;
        }
        else
        {
            _expression += character;
        }

        await CalculateIntermediate();
    }

    private async Task AppendDecimal()
    {
        if (_hasError) Clear();

        var lastNumberBlock = GetLastNumberBlock();
        if (!lastNumberBlock.Contains("."))
        {
            if (string.IsNullOrEmpty(_expression) || IsOperator(_expression[^1..]))
            {
                _expression += "0.";
            }
            else
            {
                _expression += ".";
            }
        }
        await Task.CompletedTask;
    }

    private string GetLastNumberBlock()
    {
        var parts = _expression.Split(new[] { '+', '-', '*', '/' }, StringSplitOptions.None);
        return parts.Length > 0 ? parts[^1] : "";
    }

    private bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/";
    }

    private void Clear()
    {
        _expression = "";
        _result = "0";
        _hasError = false;
    }

    private async Task Backspace()
    {
        if (_hasError)
        {
            Clear();
            return;
        }

        if (_expression.Length > 0)
        {
            _expression = _expression[..^1];
            await CalculateIntermediate();
        }
    }

    private async Task ApplyKdv(decimal rate, bool add)
    {
        try
        {
            await CalculateFinal();
            if (decimal.TryParse(_result, NumberStyles.Any, TrCulture, out var currentVal))
            {
                decimal newVal;
                if (add)
                {
                    newVal = currentVal * (1 + (rate / 100));
                }
                else
                {
                    newVal = currentVal / (1 + (rate / 100));
                }

                _expression = Math.Round(newVal, 2).ToString(CultureInfo.InvariantCulture);
                _result = Math.Round(newVal, 2).ToString("N2", TrCulture);
            }
        }
        catch
        {
            _hasError = true;
            _result = "Hata";
        }
    }

    private async Task CalculateIntermediate()
    {
        if (string.IsNullOrEmpty(_expression))
        {
            _result = "0";
            return;
        }

        var lastChar = _expression[^1..];
        if (IsOperator(lastChar) || lastChar == ".")
        {
            return;
        }

        await PerformCalculation(false);
    }

    private async Task CalculateFinal()
    {
        if (string.IsNullOrEmpty(_expression))
        {
            return;
        }

        var lastChar = _expression[^1..];
        if (IsOperator(lastChar))
        {
            _expression = _expression[..^1];
        }

        await PerformCalculation(true);
    }

    private async Task PerformCalculation(bool isFinal)
    {
        try
        {
            var calculated = await Js.InvokeAsync<double>("window.turbohesap.evaluateExpression", _expression);
            
            if (double.IsNaN(calculated) || double.IsInfinity(calculated))
            {
                _hasError = true;
                _result = "Hata";
            }
            else
            {
                var decimalVal = (decimal)calculated;
                _result = decimalVal.ToString("N2", TrCulture);
                if (isFinal)
                {
                    _expression = decimalVal.ToString(CultureInfo.InvariantCulture);
                }
            }
        }
        catch
        {
            if (isFinal)
            {
                _hasError = true;
                _result = "Hata";
            }
        }
    }

    private async Task SubmitValue()
    {
        await CalculateFinal();
        if (!_hasError && decimal.TryParse(_result, NumberStyles.Any, TrCulture, out var finalVal))
        {
            await OnValueSelected.InvokeAsync(Math.Round(finalVal, 2));
        }
    }

    private async Task HandleKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
    {
        var key = e.Key;
        if (key.Length == 1 && key[0] >= '0' && key[0] <= '9')
        {
            await AppendCharacter(key);
        }
        else if (key == "+" || key == "-" || key == "*" || key == "/")
        {
            await AppendCharacter(key);
        }
        else if (key == "," || key == ".")
        {
            await AppendDecimal();
        }
        else if (key == "Enter")
        {
            if (e.CtrlKey)
            {
                await SubmitValue();
            }
            else
            {
                await CalculateFinal();
            }
        }
        else if (key == "=")
        {
            await CalculateFinal();
        }
        else if (key == "Backspace")
        {
            await Backspace();
        }
        else if (key == "Delete" || key == "Del")
        {
            Clear();
        }
        else if (key == "Escape")
        {
            await OnCancel.InvokeAsync();
        }
        else if (key.Equals("c", StringComparison.OrdinalIgnoreCase))
        {
            Clear();
        }
    }
}
