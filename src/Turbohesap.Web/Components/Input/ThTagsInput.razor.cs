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
/// Kullanıcının etiketleri virgül veya enter tuşuyla eklediği ve backspace ile silebildiği çoklu etiket/kategori giriş kutusu.
/// </summary>
public partial class ThTagsInput : InputBase<List<string>>
{
    [Parameter] public string? Placeholder { get; set; } = "Etiket ekleyin...";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public int MaxTags { get; set; } = 0; // 0 = Sınırsız

    private string _inputValue = string.Empty;
    private bool _isFocused = false;
    private bool _preventDefault = false;

    private List<string> DisplayTags => Value ?? new List<string>();

    private string RootClass => Cx(
        "th-tags-input",
        _isFocused ? "th-tags-input--focused" : "",
        SizeClass(Size, "th-tags-input"),
        Disabled ? "th-tags-input--disabled" : "",
        HasError ? "th-tags-input--error" : "",
        Class);

    private bool HasError => EditContext != null && FieldIdentifier.Model != null && EditContext.GetValidationMessages(FieldIdentifier).Any();

    private async Task HandleKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
    {
        if (Disabled) return;

        _preventDefault = false;

        if (e.Key == "Enter" || e.Key == ",")
        {
            _preventDefault = true;
            var tag = _inputValue.Trim().TrimEnd(',');
            if (!string.IsNullOrEmpty(tag))
            {
                var currentList = Value != null ? new List<string>(Value) : new List<string>();
                
                // Çift eklemeyi engelle ve maksimum etiket limitine sadık kal
                if (!currentList.Contains(tag, StringComparer.OrdinalIgnoreCase) && (MaxTags <= 0 || currentList.Count < MaxTags))
                {
                    currentList.Add(tag);
                    Value = currentList;
                    await ValueChanged.InvokeAsync(currentList);
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }
            _inputValue = string.Empty;
        }
        else if (e.Key == "Backspace" && string.IsNullOrEmpty(_inputValue))
        {
            _preventDefault = true;
            if (Value != null && Value.Count > 0)
            {
                var currentList = new List<string>(Value);
                currentList.RemoveAt(currentList.Count - 1);
                Value = currentList;
                await ValueChanged.InvokeAsync(currentList);
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }
    }

    private async Task RemoveTag(string tag)
    {
        if (Disabled || Value == null) return;

        var currentList = new List<string>(Value);
        if (currentList.Remove(tag))
        {
            Value = currentList;
            await ValueChanged.InvokeAsync(currentList);
            EditContext?.NotifyFieldChanged(FieldIdentifier);
        }
    }

    private void OnFocus() => _isFocused = true;
    private void OnBlur() => _isFocused = false;

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out List<string> result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        // Metinden doğrudan liste dönüşümü virgülle ayrılmış kelimelerle olabilir
        if (string.IsNullOrWhiteSpace(value))
        {
            result = new List<string>();
        }
        else
        {
            result = value.Split(',')
                          .Select(t => t.Trim())
                          .Where(t => !string.IsNullOrEmpty(t))
                          .ToList();
        }
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
