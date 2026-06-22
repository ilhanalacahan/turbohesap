using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Erişilebilir ve özelleştirilmiş switch (th-switch) bileşeni.
/// </summary>
public partial class ThSwitch : InputBase<bool>
{
    [Parameter] public string? Label { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string RootClass => Cx(
        "th-switch",
        SizeClass(Size, "th-switch"),
        EditContext?.FieldCssClass(FieldIdentifier),
        Class);

    private void Toggle()
    {
        if (!Disabled)
        {
            CurrentValue = !CurrentValue;
        }
    }

    protected override bool TryParseValueFromString(
         string? value,
         out bool result,
         [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (bool.TryParse(value, out result))
        {
            validationErrorMessage = null;
            return true;
        }
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
