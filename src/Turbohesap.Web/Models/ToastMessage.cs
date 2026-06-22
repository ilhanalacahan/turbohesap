namespace Turbohesap.Web.Models;

public enum ToastLevel { Info, Success, Warning, Danger }

public enum ToastPosition
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft,
    TopCenter,
    BottomCenter
}

public sealed class ToastAction
{
    public required string Text { get; init; }
    public string? Icon { get; init; }
    public string VariantClass { get; init; } = "th-btn--secondary";
    public required Action OnClick { get; init; }
}

/// <summary>Tek bir bildirim (toast). Pozisyon, ilerleme çubuğu, eylem butonları ve sabitlik desteğiyle.</summary>
public sealed class ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Message { get; set; }
    public string? Title { get; set; }
    public ToastLevel Level { get; set; } = ToastLevel.Info;
    public int DurationMs { get; set; } = 4000;
    
    public string? Icon { get; set; }
    public ToastPosition Position { get; set; } = ToastPosition.BottomRight;
    public bool ShowProgress { get; set; } = false;
    public double? Progress { get; set; }
    public bool Fixed { get; set; } = false;
    public List<ToastAction>? Actions { get; set; }

    public string IconClass => Icon == "none" ? string.Empty : (!string.IsNullOrEmpty(Icon) ? Icon : Level switch
    {
        ToastLevel.Success => "fa-solid fa-circle-check",
        ToastLevel.Warning => "fa-solid fa-triangle-exclamation",
        ToastLevel.Danger => "fa-solid fa-circle-xmark",
        _ => "fa-solid fa-circle-info"
    });

    public string CssClass => Level switch
    {
        ToastLevel.Success => "th-toast--success",
        ToastLevel.Warning => "th-toast--warning",
        ToastLevel.Danger => "th-toast--danger",
        _ => "th-toast--info"
    };
}
