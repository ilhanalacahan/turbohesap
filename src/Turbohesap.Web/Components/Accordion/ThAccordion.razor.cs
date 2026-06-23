using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Accordion;

/// <summary>
/// Açılır/kapanır içerik grupları. <c>th-accordion</c> sarmalayıcısı; alt
/// <see cref="ThAccordionItem"/> öğelerinin açık/kapalı durumunu yönetir.
/// </summary>
public partial class ThAccordion : TurboComponentBase
{
    /// <summary><see cref="ThAccordionItem"/> öğeleri.</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>Görsel varyant (Bordered/Separated/Flush).</summary>
    [Parameter] public AccordionVariant Variant { get; set; } = AccordionVariant.Bordered;

    /// <summary>Boyut.</summary>
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    /// <summary>Birden çok öğenin aynı anda açık kalmasına izin ver.</summary>
    [Parameter] public bool Multiple { get; set; }

    private readonly List<ThAccordionItem> _items = [];
    private readonly HashSet<Guid> _openIds = [];

    private string RootClass => Cx(
        "th-accordion",
        VariantClass,
        SizeClass(Size, "th-accordion"),
        Class);

    private string VariantClass => Variant switch
    {
        AccordionVariant.Separated => "th-accordion--separated",
        AccordionVariant.Flush => "th-accordion--flush",
        _ => "th-accordion--bordered"
    };

    internal void RegisterItem(ThAccordionItem item)
    {
        if (_items.Contains(item)) return;
        _items.Add(item);

        if (item.Open && !item.Disabled)
        {
            if (!Multiple) _openIds.Clear();
            _openIds.Add(item.Id);
        }
    }

    internal void UnregisterItem(ThAccordionItem item)
    {
        _items.Remove(item);
        _openIds.Remove(item.Id);
    }

    internal bool IsOpen(ThAccordionItem item) => _openIds.Contains(item.Id);

    internal void Toggle(ThAccordionItem item)
    {
        if (item.Disabled) return;

        if (_openIds.Contains(item.Id))
        {
            _openIds.Remove(item.Id);
        }
        else
        {
            if (!Multiple) _openIds.Clear();
            _openIds.Add(item.Id);
        }

        foreach (var i in _items)
        {
            i.Refresh();
        }
    }
}
