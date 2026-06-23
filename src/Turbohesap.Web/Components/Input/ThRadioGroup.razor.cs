using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Turbohesap.Web.Components.Input;

public enum RadioLayout
{
    Horizontal,
    Vertical
}

/// <summary>
/// İlgili radyo seçeneklerini sarmalayan ve yerleşimi yöneten ThRadioGroup bileşeni.
/// </summary>
public partial class ThRadioGroup<TValue> : InputBase<TValue>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RadioLayout Layout { get; set; } = RadioLayout.Horizontal;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    /// <summary>CascadingValue olarak alt ThRadio'lara kendisini iletir.</summary>
    internal string GroupName { get; } = Guid.NewGuid().ToString("N");

    private string RootClass => Cx(
        "th-radio-group",
        Layout == RadioLayout.Vertical ? "th-radio-group--vertical" : "th-radio-group--horizontal",
        Class);

    internal bool IsSelected(TValue value)
    {
        return Equals(Value, value);
    }

    internal async Task SelectValueAsync(TValue value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
        EditContext?.NotifyFieldChanged(FieldIdentifier);
    }

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out TValue result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        try
        {
            if (BindConverter.TryConvertTo<TValue>(value, System.Globalization.CultureInfo.CurrentCulture, out var parsedValue))
            {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }
        }
        catch
        {
            // ignored
        }

        result = default!;
        validationErrorMessage = "Değer dönüştürülemedi.";
        return false;
    }

    protected static string Cx(params string?[] classes)
        => string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));
}
