using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Customers;

namespace Turbohesap.Shared.Services;

/// <summary>
/// Müşteri iş mantığı sözleşmesi. Tüm mantık servis katmanında ve transactional çalışır;
/// okuma metotları AsNoTracking kullanır (req 10). Arayüz Shared'de, uygulama API'dedir (req 1).
/// </summary>
public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> GetListAsync(GetCustomersQuery query, CancellationToken cancellationToken = default);

    Task<CustomerDto?> GetByIdAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken = default);

    Task<CustomerDto> CreateAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default);

    Task<CustomerDto> UpdateAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default);
}
