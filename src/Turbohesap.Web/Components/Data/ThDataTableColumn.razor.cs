using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Turbohesap.Web.Components.Data;

/// <summary>
/// <see cref="ThDataTable{TItem}"/> için deklaratif kolon tanımı. Başlık metni, sıralama,
/// özellik erişimi veya şablon, genişlik ve görünürlük kontrolü sağlar.
/// </summary>
public partial class ThDataTableColumn<TItem> : IDisposable
{
    [CascadingParameter] public ThDataTable<TItem> Parent { get; set; } = default!;

    /// <summary>Kolon başlığı.</summary>
    [Parameter] public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Sıralama anahtarı. Doluysa başlık tıklanabilir olur; <see cref="Sortable"/>
    /// ile manuel olarak kapatılabilir.
    /// </summary>
    [Parameter] public string? SortKey { get; set; }

    /// <summary>Bu kolon sıralanabilir mi.</summary>
    [Parameter] public bool Sortable { get; set; } = true;

    /// <summary>
    /// Hücre değerini döndüren özellik ifadesi. <see cref="Template"/>
    /// verilmezse kullanılır; değer <c>null</c> ise "—" gösterilir.
    /// </summary>
    [Parameter] public Expression<Func<TItem, object?>>? Property { get; set; }

    /// <summary>Hücre içeriği (Property yerine geçer).</summary>
    [Parameter] public RenderFragment<TItem>? Template { get; set; }

    /// <summary>Varsayılan hücre içeriği şablonu (çocuk öğe olarak).</summary>
    [Parameter] public RenderFragment<TItem>? ChildContent { get; set; }

    /// <summary>Kolon genişliği (ör. <c>12rem</c>, <c>20%</c>).</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Kolon seçicide gösterilsin mi.</summary>
    [Parameter] public bool ShowInSelector { get; set; } = true;

    /// <summary>Kolon görünür mü.</summary>
    [Parameter] public bool Visible { get; set; } = true;

    public Guid Id { get; } = Guid.NewGuid();

    private Func<TItem, object?>? _getter;

    /// <summary>Verilen öğe için hücre değerini döndürür.</summary>
    internal object? GetValue(TItem item)
    {
        if (ChildContent is not null || Template is not null) return null; // Şablon doğrudan render edilir.
        if (_getter is null && Property is not null)
        {
            _getter = Property.Compile();
        }

        return _getter?.Invoke(item);
    }

    internal bool IsSortable => Sortable && !string.IsNullOrEmpty(SortKey);

    internal void ToggleVisible()
    {
        Visible = !Visible;
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException(
                "ThDataTableColumn bileşeni bir ThDataTable bileşeni içinde tanımlanmalıdır.");
        }

        Parent.RegisterColumn(this);
    }

    public void Dispose()
    {
        Parent?.UnregisterColumn(this);
        GC.SuppressFinalize(this);
    }
}
