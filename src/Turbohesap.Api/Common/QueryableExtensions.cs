using Microsoft.EntityFrameworkCore;
using Turbohesap.Shared.Common;

namespace Turbohesap.Api.Common;

/// <summary>Sayfalama yardımcıları. Tüm liste sorguları bunları kullanır (req 13).</summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Filtrelenmiş ve sıralanmış (ve genellikle DTO'ya projekte edilmiş) bir sorguyu
    /// sayfalanmış sonuca dönüştürür. Toplam sayım ve sayfa, tek round-trip mantığıyla alınır.
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, request.Page, request.PageSize);
    }
}
