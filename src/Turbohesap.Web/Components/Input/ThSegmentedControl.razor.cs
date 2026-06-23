using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Düğme görünümlü, yan yana hizalanan ve aktif seçeneği kayan efektle gösteren estetik seçim bileşeni.
/// </summary>
public partial class ThSegmentedControl<TValue> : InputBase<TValue> where TValue : notnull
{
    [Parameter] public Dictionary<TValue, string> Options { get; set; } = new();
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private int SelectedIndex
    {
        get
        {
            if (Options == null || Options.Count == 0 || Value == null) return 0;
            var keys = Options.Keys.ToList();
            var index = keys.IndexOf(Value);
            return index >= 0 ? index : 0;
        }
    }

    private string RootClass => Cx(
        "th-segmented-control",
        SizeClass(Size, "th-segmented-control"),
        Disabled ? "th-segmented-control--disabled" : "",
        HasError ? "th-segmented-control--error" : "",
        Class);

    private bool HasError => EditContext != null && FieldIdentifier.Model != null && EditContext.GetValidationMessages(FieldIdentifier).Any();

    private async Task SelectOption(TValue key)
    {
        if (Disabled) return;
        Value = key;
        await ValueChanged.InvokeAsync(key);
        EditContext?.NotifyFieldChanged(FieldIdentifier);
    }

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out TValue result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        try
        {
            if (BindConverter.TryConvertTo<TValue>(value, System.Globalization.CultureInfo.InvariantCulture, out var convertedValue))
            {
                result = convertedValue;
                validationErrorMessage = null;
                return true;
            }
        }
        catch
        {
            // ignored
        }

        result = default!;
        validationErrorMessage = "Geçersiz değer.";
        return false;
    }

    protected static string Cx(params string?[] classes)
        => string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));

    protected static string SizeClass(ComponentSize size, string prefix) => size switch
    {
        ComponentSize.Sm => $"{prefix}--sm",
        ComponentSize.Lg => $"{prefix}--lg",
        _ => $"{prefix}--md"
    };
}
