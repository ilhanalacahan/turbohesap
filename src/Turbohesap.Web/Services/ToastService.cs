using Turbohesap.Web.Models;

namespace Turbohesap.Web.Services;

/// <summary>Bildirim (toast) kuyruğu. ThToastHost bileşeni dinler ve gösterir.</summary>
public sealed class ToastService
{
    private readonly List<ToastMessage> _toasts = [];
    private readonly object _lock = new();

    public IReadOnlyList<ToastMessage> Toasts
    {
        get
        {
            lock (_lock)
            {
                return _toasts.ToList();
            }
        }
    }

    public event Action? OnChange;

    public void Show(ToastMessage toast)
    {
        lock (_lock)
        {
            _toasts.Add(toast);
        }
        OnChange?.Invoke();
    }

    public void Info(string message, string? title = null, int durationMs = 4000, bool isFixed = false, ToastPosition position = ToastPosition.BottomRight, string? icon = null, bool showProgress = false, double? progress = null, List<ToastAction>? actions = null)
        => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Info, DurationMs = durationMs, Fixed = isFixed, Position = position, Icon = icon, ShowProgress = showProgress, Progress = progress, Actions = actions });

    public void Success(string message, string? title = null, int durationMs = 4000, bool isFixed = false, ToastPosition position = ToastPosition.BottomRight, string? icon = null, bool showProgress = false, double? progress = null, List<ToastAction>? actions = null)
        => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Success, DurationMs = durationMs, Fixed = isFixed, Position = position, Icon = icon, ShowProgress = showProgress, Progress = progress, Actions = actions });

    public void Warning(string message, string? title = null, int durationMs = 4000, bool isFixed = false, ToastPosition position = ToastPosition.BottomRight, string? icon = null, bool showProgress = false, double? progress = null, List<ToastAction>? actions = null)
        => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Warning, DurationMs = durationMs, Fixed = isFixed, Position = position, Icon = icon, ShowProgress = showProgress, Progress = progress, Actions = actions });

    public void Error(string message, string? title = null, int durationMs = 4000, bool isFixed = false, ToastPosition position = ToastPosition.BottomRight, string? icon = null, bool showProgress = false, double? progress = null, List<ToastAction>? actions = null)
        => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Danger, DurationMs = durationMs, Fixed = isFixed, Position = position, Icon = icon, ShowProgress = showProgress, Progress = progress, Actions = actions });

    public void Remove(Guid id)
    {
        lock (_lock)
        {
            var index = _toasts.FindIndex(t => t.Id == id);
            if (index >= 0)
            {
                _toasts.RemoveAt(index);
            }
            else
            {
                return;
            }
        }
        OnChange?.Invoke();
    }

    public void NotifyChanged() => OnChange?.Invoke();
}
