namespace Turbohesap.Web.Models;

public enum ToastLevel { Info, Success, Warning, Danger }

/// <summary>Tek bir bildirim (toast).</summary>
public sealed class ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Message { get; init; }
    public string? Title { get; init; }
    public ToastLevel Level { get; init; } = ToastLevel.Info;
    public int DurationMs { get; init; } = 4000;

    public string IconClass => Level switch
    {
        ToastLevel.Success => "fa-solid fa-circle-check",
        ToastLevel.Warning => "fa-solid fa-triangle-exclamation",
        ToastLevel.Danger => "fa-solid fa-circle-xmark",
        _ => "fa-solid fa-circle-info"
    };

    public string CssClass => Level switch
    {
        ToastLevel.Success => "th-toast--success",
        ToastLevel.Warning => "th-toast--warning",
        ToastLevel.Danger => "th-toast--danger",
        _ => "th-toast--info"
    };
}
