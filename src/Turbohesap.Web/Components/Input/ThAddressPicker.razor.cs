using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Input;

/// <summary>
/// Depo yönetiminde raf adreslerini (Bölge-Koridor-Raf-Kat) görsel olarak seçtiren gelişmiş giriş bileşeni.
/// React Portal benzeri fixed konumlandırma kullanarak overflow: hidden engeline takılmaz.
/// Değere göre limitlerini ve listelerini dinamik olarak genişletebilir.
/// </summary>
public partial class ThAddressPicker : InputBase<string>, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public string? Placeholder { get; set; } = "Depo Adresi Seçin";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    // Grid sınırları ve özelleştirme
    [Parameter] public List<string> Areas { get; set; } = new() { "A", "B", "C", "D" };
    [Parameter] public int MaxRows { get; set; } = 9; // Koridorlar (01 - 09)
    [Parameter] public List<string> Shelves { get; set; } = new() { "A", "B", "C", "D", "E", "F" }; // Raflar (Yatay kolonlar)
    [Parameter] public int MaxLevels { get; set; } = 5; // Katlar (1 - 5, Dikey satırlar)

    private bool _isDropdownOpen;
    private string _selectedArea = "A";
    private int _selectedRow = 1;
    private string _selectedShelf = "A";
    private int _selectedLevel = 1;

    // Lokalde yönetilen dinamik sınırlar (Blazor parameter overwriting korumalı)
    private List<string> _activeAreas = new();
    private List<string> _activeShelves = new();
    private int _activeMaxRows;
    private int _activeMaxLevels;

    private ElementReference _triggerRef;
    private ElementReference _menuRef;
    private IJSObjectReference? _jsHandler;
    private DotNetObjectReference<ThAddressPicker>? _dotNetRef;
    private readonly string _id = Guid.NewGuid().ToString();

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        
        // Parametreleri yerel kopyalara al
        _activeAreas = Areas?.ToList() ?? new List<string> { "A", "B", "C", "D" };
        _activeShelves = Shelves?.ToList() ?? new List<string> { "A", "B", "C", "D", "E", "F" };
        _activeMaxRows = MaxRows;
        _activeMaxLevels = MaxLevels;

        ParseAddress(Value);
    }

    private string RootClass => Cx(
        "th-address-picker",
        _isDropdownOpen ? "th-address-picker--open" : "",
        SizeClass(Size, "th-address-picker"),
        Disabled ? "th-address-picker--disabled" : "",
        HasError ? "th-address-picker--error" : "",
        Class);

    private bool HasError => EditContext != null && FieldIdentifier.Model != null && EditContext.GetValidationMessages(FieldIdentifier).Any();

    private void ToggleDropdown()
    {
        if (Disabled) return;
        if (_isDropdownOpen)
        {
            CloseMenu();
        }
        else
        {
            _isDropdownOpen = true;
            ParseAddress(Value);
        }
    }

    [JSInvokable]
    public void CloseMenu()
    {
        _isDropdownOpen = false;
        StateHasChanged();
        _ = CleanupJSAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isDropdownOpen && _jsHandler == null)
        {
            try
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                _jsHandler = await JS.InvokeAsync<IJSObjectReference>("turbohesap.menu.initDropdown", _id, _triggerRef, _menuRef, _dotNetRef);
            }
            catch
            {
                // Fail silent
            }
        }
    }

    private void ParseAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            _selectedArea = _activeAreas.FirstOrDefault() ?? "A";
            _selectedRow = 1;
            _selectedShelf = _activeShelves.FirstOrDefault() ?? "A";
            _selectedLevel = 1;
            return;
        }

        var parts = address.Split('-');
        
        // 1. Bölge (Alan) - Dinamik Ekleme
        if (parts.Length >= 1)
        {
            var area = parts[0].ToUpperInvariant().Trim();
            if (!string.IsNullOrWhiteSpace(area))
            {
                if (!_activeAreas.Contains(area))
                {
                    _activeAreas.Add(area);
                    _activeAreas = _activeAreas.Distinct().OrderBy(a => a).ToList();
                }
                _selectedArea = area;
            }
        }
        else
        {
            _selectedArea = _activeAreas.FirstOrDefault() ?? "A";
        }

        // 2. Koridor (Aks) - Dinamik Limit Genişletme
        if (parts.Length >= 2 && int.TryParse(parts[1], out var r))
        {
            if (r > _activeMaxRows)
            {
                _activeMaxRows = r;
            }
            _selectedRow = Math.Clamp(r, 1, _activeMaxRows);
        }
        else
        {
            _selectedRow = 1;
        }

        // 3. Raf (Yatay Sütun) - Dinamik Ekleme
        if (parts.Length >= 3)
        {
            var shelf = parts[2].ToUpperInvariant().Trim();
            if (!string.IsNullOrWhiteSpace(shelf))
            {
                if (!_activeShelves.Contains(shelf))
                {
                    _activeShelves.Add(shelf);
                    _activeShelves = _activeShelves.Distinct().OrderBy(s => s).ToList();
                }
                _selectedShelf = shelf;
            }
        }
        else
        {
            _selectedShelf = _activeShelves.FirstOrDefault() ?? "A";
        }

        // 4. Kat (Dikey Satır) - Dinamik Limit Genişletme
        if (parts.Length >= 4 && int.TryParse(parts[3], out var l))
        {
            if (l > _activeMaxLevels)
            {
                _activeMaxLevels = l;
            }
            _selectedLevel = Math.Clamp(l, 1, _activeMaxLevels);
        }
        else
        {
            _selectedLevel = 1;
        }
    }

    private async Task SelectCell(string shelf, int level)
    {
        _selectedShelf = shelf;
        _selectedLevel = level;
        await UpdateValue();
        CloseMenu();
    }

    private async Task ChangeRow(int delta)
    {
        _selectedRow = Math.Clamp(_selectedRow + delta, 1, _activeMaxRows);
        await UpdateValue();
    }

    private async Task SelectArea(string area)
    {
        _selectedArea = area;
        await UpdateValue();
    }

    private async Task UpdateValue()
    {
        var newValue = $"{_selectedArea}-{_selectedRow:D2}-{_selectedShelf}-{_selectedLevel:D2}";
        Value = newValue;
        await ValueChanged.InvokeAsync(newValue);
        EditContext?.NotifyFieldChanged(FieldIdentifier);
    }

    private async Task ClearValue()
    {
        if (Disabled) return;
        Value = string.Empty;
        await ValueChanged.InvokeAsync(string.Empty);
        EditContext?.NotifyFieldChanged(FieldIdentifier);
        ParseAddress(null);
    }

    protected override bool TryParseValueFromString(
        string? value,
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out string result,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value!;
        validationErrorMessage = null;
        return true;
    }

    protected static string Cx(params string?[] classes)
        => string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));

    protected static string SizeClass(ComponentSize size, string prefix) => size switch
    {
        ComponentSize.Sm => $"{prefix}--sm",
        ComponentSize.Lg => $"{prefix}--lg",
        _ => $"{prefix}--md"
    };

    private async Task CleanupJSAsync()
    {
        if (_jsHandler != null)
        {
            try
            {
                await _jsHandler.InvokeVoidAsync("dispose");
                await _jsHandler.DisposeAsync();
            }
            catch
            {
                // Fail silent
            }
            _jsHandler = null;
        }
        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }

    public async ValueTask DisposeAsync()
    {
        await CleanupJSAsync();
        GC.SuppressFinalize(this);
    }
}
