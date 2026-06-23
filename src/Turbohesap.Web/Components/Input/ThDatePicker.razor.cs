using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Turbohesap.Web.Components.Base;
using System.Globalization;

namespace Turbohesap.Web.Components.Input;

public enum DatePickerType
{
    Date,
    Time,
    DateTime
}

/// <summary>
/// Tarih, saat veya tarih-saat seçimi yapabilen estetik (th-datepicker) bileşeni.
/// </summary>
public partial class ThDatePicker<TValue> : InputBase<TValue>
{
    [Parameter] public DatePickerType Type { get; set; } = DatePickerType.Date;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    
    /// <summary>Sol tarafta görüntülenecek ikon sınıfını ezmek için kullanılır. Boşsa tipe göre otomatik atanır.</summary>
    [Parameter] public string? LeadingIcon { get; set; }

    private string InputType => Type switch
    {
        DatePickerType.Time => "time",
        DatePickerType.DateTime => "datetime-local",
        _ => "date"
    };

    private string IconClass => !string.IsNullOrWhiteSpace(LeadingIcon) 
        ? LeadingIcon 
        : (Type == DatePickerType.Time ? "fa-regular fa-clock" : "fa-regular fa-calendar");

    private string FormattedValue
    {
        get
        {
            if (Value is null) return string.Empty;

            if (Value is DateTime dt)
            {
                return FormatDateTime(dt);
            }
            else if (Value is DateTimeOffset dto)
            {
                return FormatDateTime(dto.DateTime);
            }
            else if (Value is TimeOnly to)
            {
                return to.ToString("HH:mm", CultureInfo.InvariantCulture);
            }
            else if (Value is DateOnly doVal)
            {
                return doVal.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return Value.ToString() ?? string.Empty;
        }
    }

    private string FormatDateTime(DateTime dt)
    {
        return Type switch
        {
            DatePickerType.Time => dt.ToString("HH:mm", CultureInfo.InvariantCulture),
            DatePickerType.DateTime => dt.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
            _ => dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        };
    }

    private string RootClass => Cx(
        "th-input-wrapper-outer",
        SizeClass(Size, "th-input-wrapper-outer"),
        Disabled ? "th-input-wrapper-outer--disabled" : "",
        HasError ? "th-input-wrapper-outer--error" : "",
        Class);

    private bool HasError => EditContext != null && FieldIdentifier.Model != null && EditContext.GetValidationMessages(FieldIdentifier).Any();

    private async Task HandleChange(ChangeEventArgs e)
    {
        var stringValue = e.Value?.ToString();
        
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            Value = default!;
            await ValueChanged.InvokeAsync(Value);
            EditContext?.NotifyFieldChanged(FieldIdentifier);
            return;
        }

        if (TryParseValue(stringValue, out var result))
        {
            Value = result;
            await ValueChanged.InvokeAsync(Value);
            EditContext?.NotifyFieldChanged(FieldIdentifier);
        }
    }

    private bool TryParseValue(string stringValue, out TValue result)
    {
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

        try
        {
            if (targetType == typeof(DateTime))
            {
                if (DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                {
                    result = (TValue)(object)dt;
                    return true;
                }
                if (DateTime.TryParse(stringValue, out var dtNormal))
                {
                    result = (TValue)(object)dtNormal;
                    return true;
                }
            }
            else if (targetType == typeof(DateTimeOffset))
            {
                if (DateTimeOffset.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dto))
                {
                    result = (TValue)(object)dto;
                    return true;
                }
                if (DateTimeOffset.TryParse(stringValue, out var dtoNormal))
                {
                    result = (TValue)(object)dtoNormal;
                    return true;
                }
            }
            else if (targetType == typeof(DateOnly))
            {
                if (DateOnly.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var doVal))
                {
                    result = (TValue)(object)doVal;
                    return true;
                }
            }
            else if (targetType == typeof(TimeOnly))
            {
                if (TimeOnly.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var toVal))
                {
                    result = (TValue)(object)toVal;
                    return true;
                }
            }
        }
        catch
        {
            // ignored
        }

        try
        {
            if (BindConverter.TryConvertTo<TValue>(stringValue, CultureInfo.InvariantCulture, out var convertedValue))
            {
                result = convertedValue;
                return true;
            }
        }
        catch
        {
            // ignored
        }

        result = default!;
        return false;
    }

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out TValue result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default!;
            validationErrorMessage = null;
            return true;
        }

        if (TryParseValue(value, out var parsedValue))
        {
            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = default!;
        validationErrorMessage = "Geçersiz tarih/saat formatı.";
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
