using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// ThRadioGroup altında yer alan tekil radyo seçeneği (th-radio).
/// </summary>
public partial class ThRadio<TValue> : ComponentBase
{
    [CascadingParameter] internal ThRadioGroup<TValue>? RadioGroup { get; set; }

    [Parameter] public TValue? Value { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    private bool IsChecked => RadioGroup != null && Value != null && RadioGroup.IsSelected(Value);
    private bool IsDisabled => Disabled || (RadioGroup != null && RadioGroup.Disabled);
    private string RadioGroupName => RadioGroup?.GroupName ?? string.Empty;

    private string RootClass => Cx(
        "th-radio",
        IsDisabled ? "th-radio--disabled" : "",
        IsChecked ? "th-radio--checked" : "",
        Class);

    private async Task HandleChange()
    {
        if (RadioGroup != null && Value != null && !IsDisabled)
        {
            await RadioGroup.SelectValueAsync(Value);
        }
    }

    protected static string Cx(params string?[] classes)
        => string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));
}
