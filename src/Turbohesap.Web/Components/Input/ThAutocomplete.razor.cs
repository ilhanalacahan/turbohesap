using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;
using System.Timers;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Statik liste veya REST servisi destekleyen, şablonlanabilir, gecikmeli (debounce) otomatik tamamlama (autocomplete) bileşeni.
/// </summary>
public partial class ThAutocomplete<TItem> : InputBase<TItem>, IDisposable
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter] public IEnumerable<TItem>? Items { get; set; }
    [Parameter] public Func<string, Task<IEnumerable<TItem>>>? SearchFunc { get; set; }
    [Parameter] public Func<TItem, string>? ItemTextFunc { get; set; }
    [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
    
    [Parameter] public int MinLength { get; set; } = 1;
    [Parameter] public int DebounceMs { get; set; } = 300;
    [Parameter] public bool OpenOnFocus { get; set; } = false;
    
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    /// <summary>Bir öğe seçildiğinde tetiklenen olay.</summary>
    [Parameter] public EventCallback<TItem> OnItemSelected { get; set; }

    private string? _searchText;
    private List<TItem> _suggestions = new();
    private bool _isOpen;
    private bool _isLoading;
    private System.Threading.Timer? _debounceTimer;
    private CancellationTokenSource? _cts;
    private bool _isFocused;
    private ElementReference _inputElement;
    private ElementReference _dropdownElement;
    private bool _openUpward;

    // Klavye Navigasyon Alanları
    private int _selectedIndex = -1;
    private bool _preventDefault = false;

    private string RootClass => Cx(
        "th-autocomplete",
        _isOpen ? "th-autocomplete--open" : "",
        Class);

    private string InputWrapperClass => Cx(
        "th-input-wrapper-outer",
        SizeClass(Size, "th-input-wrapper-outer"),
        Disabled ? "th-input-wrapper-outer--disabled" : "",
        HasError ? "th-input-wrapper-outer--error" : "",
        _isFocused ? "th-input-wrapper-outer--focused" : "");

    private bool HasError => EditContext != null && FieldIdentifier.Model != null && EditContext.GetValidationMessages(FieldIdentifier).Any();

    protected override void OnInitialized()
    {
        _debounceTimer = new System.Threading.Timer(OnDebounceTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    protected override void OnParametersSet()
    {
        if (Value != null)
        {
            _searchText = GetItemText(Value);
        }
        else if (string.IsNullOrEmpty(_searchText))
        {
            _searchText = string.Empty;
        }
    }

    private string GetItemText(TItem item)
    {
        if (item == null) return string.Empty;
        return ItemTextFunc != null ? ItemTextFunc(item) : item.ToString() ?? string.Empty;
    }

    private async Task HandleInput(ChangeEventArgs e)
    {
        _searchText = e.Value?.ToString();
        _selectedIndex = -1;

        if (string.IsNullOrEmpty(_searchText) || _searchText.Length < MinLength)
        {
            _suggestions.Clear();
            _isOpen = false;
            Value = default!;
            await ValueChanged.InvokeAsync(default);
            return;
        }

        _isLoading = true;
        _isOpen = true;
        
        await UpdateDropdownPositionAsync();
        _debounceTimer?.Change(DebounceMs, Timeout.Infinite);
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
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            IEnumerable<TItem> results = Array.Empty<TItem>();

            if (SearchFunc != null)
            {
                results = await SearchFunc(_searchText ?? string.Empty);
            }
            else if (Items != null)
            {
                var query = _searchText?.ToLower() ?? string.Empty;
                results = Items.Where(item => GetItemText(item).ToLower().Contains(query));
            }

            if (!token.IsCancellationRequested)
            {
                _suggestions = results.ToList();
                _selectedIndex = -1;
                _isLoading = false;
                _isOpen = _suggestions.Any();
                
                if (_isOpen)
                {
                    await UpdateDropdownPositionAsync();
                }
                
                StateHasChanged();
            }
        }
        catch (Exception)
        {
            if (!token.IsCancellationRequested)
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }

    private async Task HandleFocus()
    {
        _isFocused = true;
        if (OpenOnFocus && !Disabled)
        {
            if (string.IsNullOrEmpty(_searchText) || _searchText.Length >= MinLength)
            {
                _isOpen = true;
                _isLoading = true;
                await TriggerSearchAsync();
            }
        }
    }

    private async Task HandleBlur()
    {
        _isFocused = false;
        await Task.Delay(200);
        _isOpen = false;
        _selectedIndex = -1;
        StateHasChanged();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        _preventDefault = false;

        if (!_isOpen || !_suggestions.Any())
        {
            if (e.Key == "ArrowDown" && !Disabled)
            {
                _preventDefault = true;
                _isOpen = true;
                _isLoading = true;
                await TriggerSearchAsync();
            }
            return;
        }

        switch (e.Key)
        {
            case "ArrowDown":
                _preventDefault = true;
                _selectedIndex = (_selectedIndex + 1) % _suggestions.Count;
                StateHasChanged();
                await ScrollToActiveItemAsync();
                break;
            case "ArrowUp":
                _preventDefault = true;
                _selectedIndex = _selectedIndex <= 0 ? _suggestions.Count - 1 : _selectedIndex - 1;
                StateHasChanged();
                await ScrollToActiveItemAsync();
                break;
            case "Enter":
                _preventDefault = true;
                if (_selectedIndex >= 0 && _selectedIndex < _suggestions.Count)
                {
                    await SelectItem(_suggestions[_selectedIndex]);
                }
                break;
            case "Escape":
                _preventDefault = true;
                _isOpen = false;
                _selectedIndex = -1;
                StateHasChanged();
                break;
        }
    }

    private async Task UpdateDropdownPositionAsync()
    {
        try
        {
            _openUpward = await JSRuntime.InvokeAsync<bool>("turbohesap.checkDropdownPosition", _inputElement);
        }
        catch
        {
            _openUpward = false;
        }
    }

    private async Task ScrollToActiveItemAsync()
    {
        if (_selectedIndex >= 0)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("turbohesap.scrollActiveDropdownItem", _dropdownElement, _selectedIndex);
            }
            catch
            {
                // ignored
            }
        }
    }

    private async Task SelectItem(TItem item)
    {
        Value = item;
        _searchText = GetItemText(item);
        _isOpen = false;
        _selectedIndex = -1;
        _suggestions.Clear();
        
        await ValueChanged.InvokeAsync(item);
        await OnItemSelected.InvokeAsync(item);
        EditContext?.NotifyFieldChanged(FieldIdentifier);
    }

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out TItem result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = Value;
        validationErrorMessage = null;
        return true;
    }

    public void Dispose()
    {
        _debounceTimer?.Dispose();
        _cts?.Dispose();
    }

    protected static string Cx(params string?[] classes)
        => string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));

    protected static string SizeClass(ComponentSize size, string prefix) => size switch
    {
        ComponentSize.Sm => $"{prefix}--sm",
        ComponentSize.Lg => $"{prefix}--lg",
        _ => $"{prefix}--md"
    };
}
