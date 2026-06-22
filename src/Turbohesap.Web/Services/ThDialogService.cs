using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turbohesap.Web.Services;

/// <summary>
/// TS interop destekli, sıfır Blazor server gecikmeli dinamik dialog servisi.
/// </summary>
public class ThDialogService
{
    private readonly IJSRuntime _js;

    public ThDialogService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<ThDialogRef> ShowAsync(DialogOptions options)
    {
        var dialogId = Guid.NewGuid().ToString("N");
        var dialogRef = new ThDialogRef(_js, dialogId);
        
        // C# callbacks from JS interop
        var dotNetRef = DotNetObjectReference.Create(new DialogInteropHelper(dialogRef));

        await _js.InvokeVoidAsync("turbohesap.dialog.show", dialogId, new
        {
            type = options.Type.ToString().ToLowerInvariant(),
            variant = options.Variant.ToString().ToLowerInvariant(),
            title = options.Title,
            message = options.Message,
            placeholder = options.Placeholder,
            confirmText = options.ConfirmText ?? "Tamam",
            cancelText = options.CancelText ?? "İptal",
            duration = options.Duration,
            value = options.Value
        }, dotNetRef);

        return dialogRef;
    }

    public Task<DialogResult> AlertAsync(string title, string message, DialogVariant variant = DialogVariant.Normal)
    {
        return ShowQuickAsync(DialogType.Alert, title, message, variant);
    }

    public Task<DialogResult> SuccessAsync(string title, string message)
    {
        return ShowQuickAsync(DialogType.Success, title, message, DialogVariant.Success);
    }

    public Task<DialogResult> WarningAsync(string title, string message)
    {
        return ShowQuickAsync(DialogType.Warning, title, message, DialogVariant.Warning);
    }

    public Task<DialogResult> ErrorAsync(string title, string message)
    {
        return ShowQuickAsync(DialogType.Error, title, message, DialogVariant.Danger);
    }

    public Task<DialogResult> ConfirmAsync(string title, string message, DialogVariant variant = DialogVariant.Normal, string confirmText = "Evet", string cancelText = "Hayır")
    {
        return ShowQuickAsync(DialogType.Confirm, title, message, variant, confirmText: confirmText, cancelText: cancelText);
    }

    public Task<DialogResult> PromptAsync(string title, string message, string? placeholder = null, DialogVariant variant = DialogVariant.Normal)
    {
        return ShowQuickAsync(DialogType.Prompt, title, message, variant, placeholder: placeholder);
    }

    public async Task<ThDialogRef> LoadingAsync(string title, string message)
    {
        var options = new DialogOptions
        {
            Type = DialogType.Loading,
            Title = title,
            Message = message
        };
        return await ShowAsync(options);
    }

    public async Task<ThDialogRef> BusyAsync(string title, string message)
    {
        var options = new DialogOptions
        {
            Type = DialogType.Busy,
            Title = title,
            Message = message
        };
        return await ShowAsync(options);
    }

    public async Task<ThDialogRef> ProgressAsync(string title, string message, double initialPercentage = 0)
    {
        var options = new DialogOptions
        {
            Type = DialogType.Progress,
            Title = title,
            Message = message,
            Value = initialPercentage
        };
        return await ShowAsync(options);
    }

    public async Task<ThDialogRef> CountdownAsync(string title, string message, int durationSeconds)
    {
        var options = new DialogOptions
        {
            Type = DialogType.Countdown,
            Title = title,
            Message = message,
            Duration = durationSeconds
        };
        return await ShowAsync(options);
    }

    private async Task<DialogResult> ShowQuickAsync(
        DialogType type, 
        string title, 
        string message, 
        DialogVariant variant, 
        string? confirmText = null, 
        string? cancelText = null,
        string? placeholder = null)
    {
        var options = new DialogOptions
        {
            Type = type,
            Variant = variant,
            Title = title,
            Message = message,
            ConfirmText = confirmText,
            CancelText = cancelText,
            Placeholder = placeholder
        };
        var dialogRef = await ShowAsync(options);
        return await dialogRef.Result;
    }
}

public class DialogOptions
{
    public DialogType Type { get; set; } = DialogType.Alert;
    public DialogVariant Variant { get; set; } = DialogVariant.Normal;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Placeholder { get; set; }
    public string? ConfirmText { get; set; }
    public string? CancelText { get; set; }
    public int? Duration { get; set; }
    public double? Value { get; set; }
}

public enum DialogType
{
    Alert,
    Confirm,
    Prompt,
    Loading,
    Progress,
    Countdown,
    Success,
    Warning,
    Error,
    Busy
}

public enum DialogVariant
{
    Normal,
    Primary,
    Success,
    Warning,
    Danger,
    Info
}

public enum DialogStatus
{
    Confirmed,
    Cancelled,
    Closed
}

public class DialogResult
{
    public DialogStatus Status { get; set; }
    public string? Value { get; set; }

    public static DialogResult Confirmed(string? value = null) => new() { Status = DialogStatus.Confirmed, Value = value };
    public static DialogResult Cancelled() => new() { Status = DialogStatus.Cancelled };
    public static DialogResult Closed() => new() { Status = DialogStatus.Closed };
}

public class DialogInteropHelper
{
    private readonly ThDialogRef _dialogRef;

    public DialogInteropHelper(ThDialogRef dialogRef)
    {
        _dialogRef = dialogRef;
    }

    [JSInvokable]
    public void OnConfirm(string? value)
    {
        _dialogRef.SetResult(DialogResult.Confirmed(value));
    }

    [JSInvokable]
    public void OnCancel()
    {
        _dialogRef.SetResult(DialogResult.Cancelled());
    }

    [JSInvokable]
    public void OnClose()
    {
        _dialogRef.SetResult(DialogResult.Closed());
    }
}
