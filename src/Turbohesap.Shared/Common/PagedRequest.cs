namespace Turbohesap.Shared.Common;

/// <summary>
/// Tüm listeleme sorguları bu tipten türer. Sayfalama (zorunlu), arama ve sıralama
/// parametrelerini taşır. PageSize parametriktir ancak üst sınır ile korunur.
/// </summary>
public abstract class PagedRequest
{
    /// <summary>İzin verilen en büyük sayfa boyutu. İstemcinin DoS yapmasını engeller.</summary>
    public const int MaxPageSize = 200;

    private int _page = 1;
    private int _pageSize = 20;

    /// <summary>1 tabanlı sayfa numarası.</summary>
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    /// <summary>Sayfa başına kayıt sayısı (1..<see cref="MaxPageSize"/>).</summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
    }

    /// <summary>Serbest metin arama terimi (opsiyonel).</summary>
    public string? Search { get; set; }

    /// <summary>Sıralanacak alan adı (DTO alan adı, opsiyonel).</summary>
    public string? SortBy { get; set; }

    /// <summary>Sıralama yönü.</summary>
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

    /// <summary>EF Core sorgularında kullanılacak atlanacak kayıt sayısı.</summary>
    public int Skip => (Page - 1) * PageSize;
}
