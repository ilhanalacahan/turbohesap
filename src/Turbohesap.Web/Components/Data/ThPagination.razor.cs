using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Components.Data;

/// <summary>Sayfalama çubuğu; sayfa boyutu parametriktir.</summary>
public partial class ThPagination
{
    [Parameter] public int Page { get; set; } = 1;
    [Parameter] public int PageSize { get; set; } = 20;
    [Parameter] public int TotalCount { get; set; }
    [Parameter] public int TotalPages { get; set; }
    [Parameter] public EventCallback<int> PageChanged { get; set; }
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }
    [Parameter] public int[] PageSizeOptions { get; set; } = [10, 20, 50, 100];

    private Task GoTo(int page)
    {
        var target = Math.Clamp(page, 1, Math.Max(1, TotalPages));
        return target == Page ? Task.CompletedTask : PageChanged.InvokeAsync(target);
    }

    private Task OnPageSizeChanged(ChangeEventArgs e)
        => int.TryParse(e.Value?.ToString(), out var size) ? PageSizeChanged.InvokeAsync(size) : Task.CompletedTask;
}
