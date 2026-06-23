using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Turbohesap.Shared.Common;
using Turbohesap.Shared.Common.QueryBuilder;
using Turbohesap.Web.Components.Data;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.DataTable;

/// <summary>
/// Showcase sayfasındaki DataTable testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class DataTableShowcase : ComponentBase
{
    [Inject] private ToastService Toasts { get; set; } = default!;

    private List<ShowcaseProduct> _allProducts = [];
    private List<ShowcaseProduct> _displayProducts = [];

    // Query Builder Alanları ve Değişkenleri
    private List<QueryBuilderField> _queryBuilderFields = [
        new() { Name = "Code", Label = "Ürün Kodu", Type = QueryFieldType.String },
        new() { Name = "Name", Label = "Ürün Adı", Type = QueryFieldType.String },
        new() { Name = "Category", Label = "Kategori", Type = QueryFieldType.String },
        new() { Name = "Price", Label = "Fiyat", Type = QueryFieldType.Number },
        new() { Name = "Stock", Label = "Stok", Type = QueryFieldType.Number },
        new() { Name = "IsActive", Label = "Aktif", Type = QueryFieldType.Boolean },
        new() { Name = "UpdatedAt", Label = "Güncelleme Tarihi", Type = QueryFieldType.Date }
    ];

    private QueryGroup _queryGroup = new();
    private List<ShowcaseProduct> _filteredProducts = [];
    private string _jsonQuery = string.Empty;
    private string _sqlQuery = string.Empty;
    private string _linqQuery = string.Empty;

    private string _sortKey = "name";
    private SortDirection _sortDirection = SortDirection.Ascending;

    private int _page = 1;
    private int _pageSize = 10;
    private int _totalCount;
    private int _totalPages;

    private HashSet<ShowcaseProduct> _selected = [];
    private HashSet<ShowcaseProduct> _singleSelected = [];

    protected override void OnInitialized()
    {
        var categories = new[] { "Elektronik", "Mobilya", "Giyim", "Kırtasiye" };
        var random = new Random(42);

        _allProducts = Enumerable.Range(1, 35).Select(i => new ShowcaseProduct
        {
            Id = i,
            Code = $"PRD-{i:D3}",
            Name = $"Ürün {i}",
            Category = categories[random.Next(categories.Length)],
            Price = random.Next(50, 2000),
            Stock = random.Next(0, 250),
            IsActive = random.NextDouble() > 0.2,
            UpdatedAt = DateTime.Now.AddDays(-random.Next(0, 60))
        }).ToList();

        // Varsayılan QueryBuilder Filtreleri
        _queryGroup = new QueryGroup
        {
            Operator = QueryGroupOperator.And,
            Rules = [
                new QueryRule { Field = "Price", Operator = QueryRuleOperator.GreaterThanOrEqual, Type = QueryFieldType.Number, Value = 500 },
                new QueryRule { Field = "Category", Operator = QueryRuleOperator.Equals, Type = QueryFieldType.String, Value = "Elektronik" }
            ]
        };

        UpdateFilteredProducts();
        ApplySortAndPage();
    }

    private void ApplySortAndPage()
    {
        var query = _sortKey switch
        {
            "code" => _sortDirection == SortDirection.Ascending
                ? _allProducts.OrderBy(p => p.Code)
                : _allProducts.OrderByDescending(p => p.Code),
            "category" => _sortDirection == SortDirection.Ascending
                ? _allProducts.OrderBy(p => p.Category)
                : _allProducts.OrderByDescending(p => p.Category),
            "price" => _sortDirection == SortDirection.Ascending
                ? _allProducts.OrderBy(p => p.Price)
                : _allProducts.OrderByDescending(p => p.Price),
            "stock" => _sortDirection == SortDirection.Ascending
                ? _allProducts.OrderBy(p => p.Stock)
                : _allProducts.OrderByDescending(p => p.Stock),
            "updatedAt" => _sortDirection == SortDirection.Ascending
                ? _allProducts.OrderBy(p => p.UpdatedAt)
                : _allProducts.OrderByDescending(p => p.UpdatedAt),
            _ => _sortDirection == SortDirection.Ascending
                ? _allProducts.OrderBy(p => p.Name)
                : _allProducts.OrderByDescending(p => p.Name)
        };

        var sorted = query.ToList();
        _totalCount = sorted.Count;
        _totalPages = Math.Max(1, (int)Math.Ceiling(_totalCount / (double)_pageSize));
        _page = Math.Clamp(_page, 1, _totalPages);
        _displayProducts = sorted.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }

    private void OnSort(DataTableSortInfo info)
    {
        if (_sortKey == info.Key)
        {
            _sortDirection = info.Direction;
        }
        else
        {
            _sortKey = info.Key;
            _sortDirection = info.Direction;
        }
        ApplySortAndPage();
    }

    private void OnPageChanged(int page)
    {
        _page = page;
        ApplySortAndPage();
    }

    private void OnPageSizeChanged(int size)
    {
        _pageSize = size;
        _page = 1;
        ApplySortAndPage();
    }

    private void OnRowClick(ShowcaseProduct product)
        => Toasts.Info($"Satır tıklandı: {product.Name}", "Bilgi");

    private void OnRowDoubleClick(ShowcaseProduct product)
        => Toasts.Success($"Satır çift tıklandı: {product.Name}", "Eylem");

    private void DeleteSelected()
        => Toasts.Warning($"{_selected.Count} ürün silinecek (simülasyon)");

    private void OnQueryGroupChanged(QueryGroup group)
    {
        _queryGroup = group;
        UpdateFilteredProducts();
    }

    private void UpdateFilteredProducts()
    {
        try
        {
            _filteredProducts = _allProducts.AsQueryable().ApplyQueryGroup(_queryGroup).ToList();
        }
        catch
        {
            _filteredProducts = _allProducts;
        }

        try
        {
            _jsonQuery = System.Text.Json.JsonSerializer.Serialize(_queryGroup, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            _sqlQuery = QueryBuilderExpressionBuilder.ToSql(_queryGroup);
            _linqQuery = $"p => {QueryBuilderExpressionBuilder.ToLinqString(_queryGroup)}";
        }
        catch
        {
            _jsonQuery = "Filtreleme Hatalı";
            _sqlQuery = "Filtreleme Hatalı";
            _linqQuery = "Filtreleme Hatalı";
        }
    }

    public sealed class ShowcaseProduct
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
