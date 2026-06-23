using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Turbohesap.Web.Services;

/// <summary>
/// JavaScript tarafından oluşturulmuş olan dinamik dialog penceresinin C# referansı.
/// İlerlemeyi (progress), geri sayımı (countdown) güncellemeyi ve kapatma sonuçlarını almayı sağlar.
/// </summary>
public class ThDialogRef
{
    private readonly IJSRuntime _js;
    private readonly string _dialogId;
    private readonly TaskCompletionSource<DialogResult> _resultSource = new();

    public ThDialogRef(IJSRuntime js, string dialogId)
    {
        _js = js;
        _dialogId = dialogId;
    }

    public string DialogId => _dialogId;
    public Task<DialogResult> Result => _resultSource.Task;

    public void SetResult(DialogResult result)
    {
        _resultSource.TrySetResult(result);
    }

    public async Task UpdateProgressAsync(double percentage)
    {
        await _js.InvokeVoidAsync("turbohesap.dialog.updateProgress", _dialogId, percentage);
    }

    public async Task UpdateCountdownAsync(int secondsRemaining)
    {
        await _js.InvokeVoidAsync("turbohesap.dialog.updateCountdown", _dialogId, secondsRemaining);
    }

    public async Task CloseAsync()
    {
        await _js.InvokeVoidAsync("turbohesap.dialog.close", _dialogId);
    }
}
