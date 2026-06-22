using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Components.Data;

/// <summary>Genel veri tablosu; başlık/satır şablonları sayfadan gelir (yuvalar).</summary>
public partial class ThDataTable<TItem>
{
    [Parameter] public IReadOnlyList<TItem> Items { get; set; } = [];
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public RenderFragment<TItem>? Row { get; set; }
    [Parameter] public RenderFragment? Toolbar { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }
    [Parameter] public bool Loading { get; set; }
    [Parameter] public string EmptyText { get; set; } = "Kayıt bulunamadı.";

    /// <summary>colspan için sütun sayısı (boş/yükleniyor satırlarında kullanılır).</summary>
    [Parameter] public int ColumnCount { get; set; } = 100;
}
