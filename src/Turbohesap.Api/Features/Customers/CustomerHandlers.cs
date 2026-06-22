using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Customers;
using Turbohesap.Shared.Services;
using Wolverine.Attributes;

namespace Turbohesap.Api.Features.Customers;

/// <summary>
/// Müşteri CQRS handler'ları (req 6). Bu sınıf, komut/sorgu mesajlarını servis katmanına
/// delege eden ince CQRS sınırıdır; FluentValidation middleware'i handler öncesinde çalışır.
/// İş mantığının tamamı <see cref="ICustomerService"/> içindedir (req 10).
/// </summary>
[WolverineHandler]
public sealed class CustomerHandlers
{
    public static Task<PagedResult<CustomerDto>> Handle(
        GetCustomersQuery query, ICustomerService service, CancellationToken cancellationToken)
        => service.GetListAsync(query, cancellationToken);

    public static Task<CustomerDto?> Handle(
        GetCustomerByIdQuery query, ICustomerService service, CancellationToken cancellationToken)
        => service.GetByIdAsync(query, cancellationToken);

    public static Task<CustomerDto> Handle(
        CreateCustomerCommand command, ICustomerService service, CancellationToken cancellationToken)
        => service.CreateAsync(command, cancellationToken);

    public static Task<CustomerDto> Handle(
        UpdateCustomerCommand command, ICustomerService service, CancellationToken cancellationToken)
        => service.UpdateAsync(command, cancellationToken);

    public static Task<bool> Handle(
        DeleteCustomerCommand command, ICustomerService service, CancellationToken cancellationToken)
        => service.DeleteAsync(command, cancellationToken);
}
