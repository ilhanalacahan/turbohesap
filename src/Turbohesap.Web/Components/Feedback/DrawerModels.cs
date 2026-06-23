using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Turbohesap.Web.Components.Feedback;

public enum DrawerPosition
{
    Right,
    Left,
    Top,
    Bottom
}

public enum DrawerSize
{
    Sm,
    Md,
    Lg,
    Xl,
    Fullscreen
}

public class DrawerOptions
{
    public DrawerPosition Position { get; set; } = DrawerPosition.Right;
    public DrawerSize Size { get; set; } = DrawerSize.Md;
    public bool Expandable { get; set; } = false;
    public bool CloseOnOverlay { get; set; } = true;
    public ModalVariant Variant { get; set; } = ModalVariant.Default;
}

public class DrawerResult
{
    public bool Cancelled { get; set; }
    public object? Data { get; set; }

    public static DrawerResult Ok(object? data = null) => new() { Cancelled = false, Data = data };
    public static DrawerResult Cancel() => new() { Cancelled = true };
}

/// <summary>Çalışan dinamik yan panel (drawer) referansı. Kapatma durumunu ve sonuç değerini asenkron yönetir.</summary>
public class ThDrawerRef
{
    private readonly TaskCompletionSource<DrawerResult> _resultSource = new();

    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public RenderFragment Content { get; set; } = default!;
    public DrawerOptions Options { get; set; } = new();

    public Task<DrawerResult> Result => _resultSource.Task;

    public void Close(DrawerResult result)
    {
        _resultSource.TrySetResult(result);
    }
}
