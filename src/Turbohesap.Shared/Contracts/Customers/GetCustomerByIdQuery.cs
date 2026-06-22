using Turbohesap.Shared.Cqrs;

namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>Kimliğe göre tek müşteri getirir.</summary>
public sealed class GetCustomerByIdQuery : IQuery<CustomerDto?>
{
    public Guid Id { get; set; }

    public GetCustomerByIdQuery() { }

    public GetCustomerByIdQuery(Guid id) => Id = id;
}
