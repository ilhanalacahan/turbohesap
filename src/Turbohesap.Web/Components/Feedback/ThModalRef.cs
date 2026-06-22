using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>Çalışan dinamik modal referansı. Kapatma durumunu ve sonuç değerini asenkron yönetir.</summary>
public class ThModalRef
{
    private readonly TaskCompletionSource<ModalResult> _resultSource = new();

    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public RenderFragment Content { get; set; } = default!;
    public ModalOptions Options { get; set; } = new();

    public Task<ModalResult> Result => _resultSource.Task;

    public void Close(ModalResult result)
    {
        _resultSource.TrySetResult(result);
    }
}
