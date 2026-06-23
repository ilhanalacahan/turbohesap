using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Data;

public enum ListViewSelectionMode
{
    None,
    Single,
    Multiple
}

/// <summary>
/// Statik veya asenkron veri destekli, seçilebilir, aranabilir ve sonsuz kaydırmalı (infinite scroll) liste görünümü (th-listview).
/// </summary>
public partial class ThListView<TItem> : TurboComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter] public IEnumerable<TItem>? Items { get; set; }
    [Parameter] public Func<string, Task<IEnumerable<TItem>>>? SearchFunc { get; set; }
    [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }

    // Standart Şablon Özelleştirmeleri (Template yoksa kullanılır)
    [Parameter] public Func<TItem, string>? ItemTitle { get; set; }
    [Parameter] public Func<TItem, string>? ItemSubtitle { get; set; }
    [Parameter] public Func<TItem, string>? ItemLeftImage { get; set; }
    [Parameter] public Func<TItem, string>? ItemLeftIcon { get; set; }
    [Parameter] public Func<TItem, string>? ItemRightText { get; set; }
    [Parameter] public Func<TItem, string>? ItemRightBadge { get; set; }
    [Parameter] public Func<TItem, string>? ItemRightBadgeVariant { get; set; } // neutral, primary, success vb.

    // Arama, Seçim ve Yükleme Özellikleri
    [Parameter] public bool Searchable { get; set; } = false;
    [Parameter] public string SearchPlaceholder { get; set; } = "Ara...";
    [Parameter] public ListViewSelectionMode SelectionMode { get; set; } = ListViewSelectionMode.None;
    [Parameter] public bool ShowCheckbox { get; set; } = false;
    
    // Sonsuz Kaydırma / Scroll Yükleme
    [Parameter] public bool InfiniteScroll { get; set; } = false;
    [Parameter] public bool HasMore { get; set; } = false;
    [Parameter] public EventCallback OnLoadMore { get; set; }
    [Parameter] public string Height { get; set; } = "400px";

    // Seçim Eventleri
    [Parameter] public TItem? SelectedItem { get; set; }
    [Parameter] public EventCallback<TItem?> SelectedItemChanged { get; set; }
    [Parameter] public List<TItem> SelectedItems { get; set; } = new();
    [Parameter] public EventCallback<List<TItem>> SelectedItemsChanged { get; set; }

    private string? _searchQuery;
    private List<TItem> _displayItems = new();
    private bool _isLoading;
    private ElementReference _scrollContainer;
    private DotNetObjectReference<ThListView<TItem>>? _selfRef;
    private System.Threading.Timer? _debounceTimer;

    private string RootClass => Cx(
        "th-listview",
        Class);

    protected override void OnInitialized()
    {
        _selfRef = DotNetObjectReference.Create(this);
        _debounceTimer = new System.Threading.Timer(OnDebounceTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Items != null && SearchFunc == null)
        {
            ApplyLocalFilter();
        }
        else if (SearchFunc != null && _displayItems.Count == 0 && !_isLoading)
        {
            await TriggerSearchAsync();
        }
    }

    private void ApplyLocalFilter()
    {
        if (Items == null) return;
        
        if (string.IsNullOrWhiteSpace(_searchQuery))
        {
            _displayItems = Items.ToList();
        }
        else
        {
            var q = _searchQuery.ToLower();
            _displayItems = Items.Where(item => 
                (ItemTitle != null && ItemTitle(item)?.ToLower().Contains(q) == true) ||
                (ItemSubtitle != null && ItemSubtitle(item)?.ToLower().Contains(q) == true) ||
                item.ToString()?.ToLower().Contains(q) == true
            ).ToList();
        }
    }

    private void HandleSearchInput(ChangeEventArgs e)
    {
        _searchQuery = e.Value?.ToString();

        if (SearchFunc == null)
        {
            ApplyLocalFilter();
        }
        else
        {
            _isLoading = true;
            _debounceTimer?.Change(300, Timeout.Infinite);
        }
    }

    private void OnDebounceTimerElapsed(object? state)
    {
        InvokeAsync(async () =>
        {
            await TriggerSearchAsync();
        });
    }

    private async Task TriggerSearchAsync()
    {
        if (SearchFunc == null) return;

        _isLoading = true;
        StateHasChanged();

        try
        {
            var results = await SearchFunc(_searchQuery ?? string.Empty);
            _displayItems = results.ToList();
        }
        catch
        {
            // ignored
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && InfiniteScroll)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("turbohesap.initListViewScroll", _scrollContainer, _selfRef);
            }
            catch
            {
                // ignored
            }
        }
    }

    [JSInvokable]
    public async Task OnScrollNearBottom()
    {
        if (_isLoading || !InfiniteScroll || !HasMore) return;

        _isLoading = true;
        StateHasChanged();

        try
        {
            await OnLoadMore.InvokeAsync();
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    private async Task HandleItemClick(TItem item)
    {
        if (SelectionMode == ListViewSelectionMode.None) return;

        if (SelectionMode == ListViewSelectionMode.Single)
        {
            if (Equals(SelectedItem, item))
            {
                SelectedItem = default;
            }
            else
            {
                SelectedItem = item;
            }
            await SelectedItemChanged.InvokeAsync(SelectedItem);
        }
        else if (SelectionMode == ListViewSelectionMode.Multiple)
        {
            if (SelectedItems.Contains(item))
            {
                SelectedItems.Remove(item);
            }
            else
            {
                SelectedItems.Add(item);
            }
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        StateHasChanged();
    }

    private bool IsSelected(TItem item)
    {
        if (SelectionMode == ListViewSelectionMode.Single)
        {
            return Equals(SelectedItem, item);
        }
        if (SelectionMode == ListViewSelectionMode.Multiple)
        {
            return SelectedItems.Contains(item);
        }
        return false;
    }

    public async ValueTask DisposeAsync()
    {
        _selfRef?.Dispose();
        _debounceTimer?.Dispose();

        if (InfiniteScroll)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("turbohesap.disposeListViewScroll", _scrollContainer);
            }
            catch
            {
                // ignored
            }
        }
    }
}
