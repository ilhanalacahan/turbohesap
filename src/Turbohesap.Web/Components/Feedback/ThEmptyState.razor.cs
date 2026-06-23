using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>
/// Arama sonuçları boş çıktığında veya modüllerde henüz veri olmadığında kullanıcıyı yönlendiren görsel şablon.
/// </summary>
public partial class ThEmptyState : TurboComponentBase
{
    [Parameter] public string Icon { get; set; } = "fa-regular fa-folder-open";
    [Parameter] public string Title { get; set; } = "Veri Bulunamadı";
    [Parameter] public string? Description { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string RootClass => Cx(
        "th-empty-state",
        SizeClass(Size, "th-empty-state"),
        Class);
}
