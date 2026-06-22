using Turbohesap.Shared.Common;

namespace Turbohesap.Web.Components.Data;

/// <summary>Veri tablosu sıralama bilgisi.</summary>
public sealed record DataTableSortInfo(string Key, SortDirection Direction);
