using System.Globalization;
using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Customers;

namespace Turbohesap.Web.Services;

/// <summary>Müşteri uç noktaları için tipli API servisi. Tüm istekler ApiClient üzerinden gider.</summary>
public sealed class CustomerApiService(ApiClient api)
{
    private const string BaseUrl = "api/v1/customers";

    public async Task<PagedResult<CustomerDto>> GetListAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseUrl}?{BuildQueryString(query)}";
        return await api.GetAsync<PagedResult<CustomerDto>>(url, cancellationToken)
            ?? PagedResult<CustomerDto>.Empty(query.Page, query.PageSize);
    }

    public Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => api.GetAsync<CustomerDto>($"{BaseUrl}/{id}", cancellationToken);

    public Task<CustomerDto?> CreateAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
        => api.PostAsync<CustomerDto>(BaseUrl, command, cancellationToken);

    public Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerCommand command, CancellationToken cancellationToken = default)
        => api.PutAsync<CustomerDto>($"{BaseUrl}/{id}", command, cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => api.DeleteAsync($"{BaseUrl}/{id}", cancellationToken);

    private static string BuildQueryString(GetCustomersQuery query)
    {
        var parts = new List<string>
        {
            $"page={query.Page}",
            $"pageSize={query.PageSize}",
            $"sortDirection={query.SortDirection}"
        };

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            parts.Add($"search={Uri.EscapeDataString(query.Search)}");
        }
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            parts.Add($"sortBy={Uri.EscapeDataString(query.SortBy)}");
        }
        if (query.OnlyActive is { } onlyActive)
        {
            parts.Add($"onlyActive={onlyActive.ToString(CultureInfo.InvariantCulture)}");
        }

        return string.Join('&', parts);
    }
}
