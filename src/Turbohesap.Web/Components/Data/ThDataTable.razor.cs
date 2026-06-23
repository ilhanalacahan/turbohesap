using Microsoft.AspNetCore.Components;
using System.Linq;
using Turbohesap.Shared.Common;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

/// <summary>
/// Genel veri tablosu; başlık/satır şablonları sayfadan gelir (yuvalar).
/// Destekler: tek/çoklu satır seçimi, sıralama, sayfalama, kolon seçimi,
/// satır tıklama/çift tıklama olayları ve deklaratif kolon tanımı.
/// </summary>
public partial class ThDataTable<TItem> : TurboComponentBase
{
    // ---------- Varolan yuvalar ----------
    [Parameter] public IReadOnlyList<TItem> Items { get; set; } = [];
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public RenderFragment<TItem>? Row { get; set; }
    [Parameter] public RenderFragment? Toolbar { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public bool Loading { get; set; }
    [Parameter] public string EmptyText { get; set; } = "Kayıt bulunamadı.";

    /// <summary>colspan için sütun sayısı (boş/yükleniyor satırlarında kullanılır).</summary>
    [Parameter] public int ColumnCount { get; set; } = 100;

    // ---------- Seçim ----------
    [Parameter] public DataTableSelectionMode SelectionMode { get; set; }
    [Parameter] public HashSet<TItem> SelectedItems { get; set; } = [];
    [Parameter] public EventCallback<HashSet<TItem>> SelectedItemsChanged { get; set; }
    [Parameter] public bool SelectOnRowClick { get; set; }

    // ---------- Satır olayları ----------
    [Parameter] public EventCallback<TItem> OnRowClick { get; set; }
    [Parameter] public EventCallback<TItem> OnRowDoubleClick { get; set; }

    // ---------- Sıralama ----------
    [Parameter] public string? SortKey { get; set; }
    [Parameter] public EventCallback<string?> SortKeyChanged { get; set; }
    [Parameter] public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    [Parameter] public EventCallback<SortDirection> SortDirectionChanged { get; set; }
    [Parameter] public EventCallback<DataTableSortInfo> SortChanged { get; set; }

    // ---------- Sayfalama ----------
    [Parameter] public int? Page { get; set; }
    [Parameter] public int? PageSize { get; set; }
    [Parameter] public int? TotalCount { get; set; }
    [Parameter] public int? TotalPages { get; set; }
    [Parameter] public EventCallback<int> PageChanged { get; set; }
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }
    [Parameter] public int[] PageSizeOptions { get; set; } = [10, 20, 50, 100];

    // ---------- Kolon seçimi ----------
    [Parameter] public bool ShowColumnSelector { get; set; }

    private readonly string _tableId = Guid.NewGuid().ToString("n");
    internal readonly List<ThDataTableColumn<TItem>> _columns = [];
    private HashSet<TItem> _selectedItems = [];

    private bool HasSelection => SelectionMode != DataTableSelectionMode.None;
    private bool UseDeclarativeColumns => Header is null && Row is null && _columns.Count > 0;
    private bool RowClickable => OnRowClick.HasDelegate || OnRowDoubleClick.HasDelegate ||
                               (HasSelection && SelectOnRowClick);
    private bool ShowBuiltInPagination => Footer is null &&
                                          Page.HasValue && PageSize.HasValue &&
                                          TotalCount.HasValue && TotalPages.HasValue;

    private int DisplayColumnCount => UseDeclarativeColumns
        ? _columns.Count(c => c.Visible) + (HasSelection ? 1 : 0)
        : ColumnCount;

    private bool AllSelected => Items.Count > 0 && Items.All(i => _selectedItems.Contains(i));

    protected override void OnParametersSet()
    {
        _selectedItems = SelectedItems ?? [];
    }

    internal void RegisterColumn(ThDataTableColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            InvokeAsync(StateHasChanged);
        }
    }

    internal void UnregisterColumn(ThDataTableColumn<TItem> column)
    {
        if (_columns.Remove(column))
        {
            InvokeAsync(StateHasChanged);
        }
    }

    private bool IsSelected(TItem item) => _selectedItems.Contains(item);

    private void ToggleSelection(TItem item)
    {
        if (_selectedItems.Contains(item))
        {
            _selectedItems.Remove(item);
        }
        else
        {
            if (SelectionMode == DataTableSelectionMode.Single) _selectedItems.Clear();
            _selectedItems.Add(item);
        }

        _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
        StateHasChanged();
    }

    private void ToggleSelectAll()
    {
        if (AllSelected)
        {
            foreach (var item in Items) _selectedItems.Remove(item);
        }
        else
        {
            foreach (var item in Items) _selectedItems.Add(item);
        }

        _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
        StateHasChanged();
    }

    private void HandleRowClick(TItem item)
    {
        if (HasSelection && SelectOnRowClick) ToggleSelection(item);
        if (OnRowClick.HasDelegate) _ = OnRowClick.InvokeAsync(item);
    }

    private void HandleRowDoubleClick(TItem item)
    {
        if (OnRowDoubleClick.HasDelegate) _ = OnRowDoubleClick.InvokeAsync(item);
    }

    private string RootClass => Cx("th-table-wrap", Class);

    private string RowClass(TItem item, bool declarative)
    {
        return Cx(
            declarative ? "th-table__row" : null,
            IsSelected(item) ? "th-table__row--selected" : null,
            RowClickable ? "th-table__row--clickable" : null);
    }

    private async Task HandleSort(ThDataTableColumn<TItem> column)
    {
        if (!column.IsSortable) return;

        var ascending = global::Turbohesap.Shared.Common.SortDirection.Ascending;
        var descending = global::Turbohesap.Shared.Common.SortDirection.Descending;

        var next = SortKey == column.SortKey
            ? (SortDirection == ascending ? descending : ascending)
            : ascending;

        SortKey = column.SortKey;
        SortDirection = next;

        await SortKeyChanged.InvokeAsync(SortKey);
        await SortDirectionChanged.InvokeAsync(SortDirection);
        await SortChanged.InvokeAsync(new DataTableSortInfo(column.SortKey!, next));
    }

    private string HeaderClass(ThDataTableColumn<TItem> column)
        => Cx(column.IsSortable ? "th-th--sortable" : null, SortKey == column.SortKey ? "th-th--sorted" : null);

    private string SortIconClass(ThDataTableColumn<TItem> column)
    {
        if (SortKey != column.SortKey) return "fa-solid fa-sort th-table__sort-icon";
        return SortDirection == global::Turbohesap.Shared.Common.SortDirection.Ascending
            ? "fa-solid fa-arrow-up th-table__sort-icon th-table__sort-icon--active"
            : "fa-solid fa-arrow-down th-table__sort-icon th-table__sort-icon--active";
    }

    private void ToggleColumnVisibility(ThDataTableColumn<TItem> column)
    {
        column.ToggleVisible();
        StateHasChanged();
    }
}
