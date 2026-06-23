using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Erişilebilir ve özelleştirilmiş textarea (th-textarea) bileşeni.
/// </summary>
public partial class ThTextArea : InputBase<string>
{
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public int Rows { get; set; } = 3;
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string RootClass => Cx(
        "th-textarea",
        SizeClass(Size, "th-textarea"),
        EditContext?.FieldCssClass(FieldIdentifier),
        Class);

    private void HandleChange(ChangeEventArgs e)
    {
        CurrentValue = e.Value?.ToString();
    }

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out string result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value!;
        validationErrorMessage = null;
        return true;
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
