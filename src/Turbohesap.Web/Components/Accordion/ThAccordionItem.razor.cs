using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Accordion;

/// <summary>
/// Tek bir akordeon öğesi: başlık (tetikleyici) + açılır panel. Bir
/// <see cref="ThAccordion"/> içinde tanımlanmalıdır.
/// </summary>
public partial class ThAccordionItem : TurboComponentBase, IDisposable
{
    [CascadingParameter] public ThAccordion Parent { get; set; } = default!;

    /// <summary>Başlık metni.</summary>
    [Parameter] public string Header { get; set; } = string.Empty;

    /// <summary>Başlık altı açıklama (opsiyonel).</summary>
    [Parameter] public string? Subtitle { get; set; }

    /// <summary>Başlık ikonu (Font Awesome sınıfı, ör. <c>fa-solid fa-user</c>).</summary>
    [Parameter] public string? Icon { get; set; }

    /// <summary>Öğe devre dışı (açılamaz).</summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>Başlangıçta açık olsun mu.</summary>
    [Parameter] public bool Open { get; set; }

    /// <summary>Açılan panel içeriği.</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public Guid Id { get; } = Guid.NewGuid();

    private bool IsOpen => Parent.IsOpen(this);

    private string AriaExpanded => IsOpen ? "true" : "false";

    private string ItemClass => Cx(
        "th-accordion__item",
        IsOpen ? "th-accordion__item--open" : null,
        Disabled ? "th-accordion__item--disabled" : null,
        Class);

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException(
                "ThAccordionItem bileşeni bir ThAccordion bileşeni içinde tanımlanmalıdır.");
        }

        Parent.RegisterItem(this);
    }

    private void Toggle() => Parent.Toggle(this);

    internal void Refresh() => StateHasChanged();

    public void Dispose()
    {
        Parent?.UnregisterItem(this);
        GC.SuppressFinalize(this);
    }
}
