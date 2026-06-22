using Turbohesap.Web.Models;

namespace Turbohesap.Web.Services;

/// <summary>Bildirim (toast) kuyruğu. ThToastHost bileşeni dinler ve gösterir.</summary>
public sealed class ToastService
{
    private readonly List<ToastMessage> _toasts = [];

    public IReadOnlyList<ToastMessage> Toasts => _toasts;

    public event Action? OnChange;

    public void Show(ToastMessage toast)
    {
        _toasts.Add(toast);
        OnChange?.Invoke();
    }

    public void Info(string message, string? title = null) => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Info });
    public void Success(string message, string? title = null) => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Success });
    public void Warning(string message, string? title = null) => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Warning });
    public void Error(string message, string? title = null) => Show(new ToastMessage { Message = message, Title = title, Level = ToastLevel.Danger });

    public void Remove(Guid id)
    {
        var index = _toasts.FindIndex(t => t.Id == id);
        if (index >= 0)
        {
            _toasts.RemoveAt(index);
            OnChange?.Invoke();
        }
    }
}
