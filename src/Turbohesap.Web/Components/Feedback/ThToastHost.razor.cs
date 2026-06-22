namespace Turbohesap.Web.Components.Feedback;

/// <summary>Toast bölgesini bir kez render eder; süresi dolan toast'ları otomatik kapatır.</summary>
public partial class ThToastHost : IDisposable
{
    private readonly HashSet<Guid> _scheduled = [];

    protected override void OnInitialized() => Toasts.OnChange += HandleChange;

    private async void HandleChange()
    {
        await InvokeAsync(StateHasChanged);

        foreach (var toast in Toasts.Toasts.ToList())
        {
            if (toast.DurationMs > 0 && _scheduled.Add(toast.Id))
            {
                var id = toast.Id;
                _ = Task.Delay(toast.DurationMs).ContinueWith(_ =>
                {
                    _scheduled.Remove(id);
                    Toasts.Remove(id);
                }, TaskScheduler.Default);
            }
        }
    }

    public void Dispose() => Toasts.OnChange -= HandleChange;
}
