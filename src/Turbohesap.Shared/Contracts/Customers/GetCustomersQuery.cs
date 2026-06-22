using Turbohesap.Shared.Common;
using Turbohesap.Shared.Cqrs;

namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>
/// Müşterileri sayfalı, aranabilir ve sıralanabilir biçimde listeler (req 13).
/// <see cref="PagedRequest"/> üzerinden Page, PageSize, Search, SortBy alır.
/// </summary>
public sealed class GetCustomersQuery : PagedRequest, IQuery<PagedResult<CustomerDto>>
{
    /// <summary>Yalnızca aktif müşterileri getir (opsiyonel filtre).</summary>
    public bool? OnlyActive { get; set; }
}
