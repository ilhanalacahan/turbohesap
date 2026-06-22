namespace Turbohesap.Shared.Common;

/// <summary>
/// Sayfalanmış listeleme sonucu. Tüm liste uç noktaları bu zarfı döner (req 13).
/// </summary>
/// <typeparam name="T">Liste öğesi tipi (genellikle bir DTO).</typeparam>
public sealed class PagedResult<T>
{
    public PagedResult() { }

    public PagedResult(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    /// <summary>Geçerli sayfadaki öğeler.</summary>
    public IReadOnlyList<T> Items { get; init; } = [];

    /// <summary>Filtre uygulandıktan sonraki toplam kayıt sayısı.</summary>
    public int TotalCount { get; init; }

    /// <summary>1 tabanlı geçerli sayfa.</summary>
    public int Page { get; init; }

    /// <summary>Sayfa boyutu.</summary>
    public int PageSize { get; init; }

    /// <summary>Toplam sayfa adedi.</summary>
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPrevious => Page > 1;

    public bool HasNext => Page < TotalPages;

    public static PagedResult<T> Empty(int page, int pageSize) => new([], 0, page, pageSize);
}
