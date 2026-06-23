using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// İkonlu, butonlu veya standart biçimde kullanılabilen estetik giriş (th-input) bileşeni.
/// </summary>
public partial class ThInput : InputBase<string>
{
    [Parameter] public string? Type { get; set; } = "text";
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    
    /// <summary>Sol tarafta görüntülenecek ikon sınıfı (ör. "fa-solid fa-envelope").</summary>
    [Parameter] public string? LeadingIcon { get; set; }
    
    /// <summary>Sağ tarafta görüntülenecek ikon sınıfı (ör. "fa-solid fa-lock").</summary>
    [Parameter] public string? TrailingIcon { get; set; }
    
    /// <summary>Sol tarafa eklenecek buton vb. bileşenler.</summary>
    [Parameter] public RenderFragment? Prepend { get; set; }
    
    /// <summary>Sağ tarafa eklenecek buton vb. bileşenler.</summary>
    [Parameter] public RenderFragment? Append { get; set; }

    [Parameter] public string? Autocomplete { get; set; }

    private string RootClass => Cx(
        "th-input-wrapper-outer",
        SizeClass(Size, "th-input-wrapper-outer"),
        Disabled ? "th-input-wrapper-outer--disabled" : "",
        HasError ? "th-input-wrapper-outer--error" : "",
        Class);

    private bool HasError => EditContext != null && FieldIdentifier.Model != null && EditContext.GetValidationMessages(FieldIdentifier).Any();

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
