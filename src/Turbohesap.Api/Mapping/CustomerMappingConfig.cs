using Mapster;
using Turbohesap.Api.Entities;
using Turbohesap.Shared.Contracts.Customers;

namespace Turbohesap.Api.Mapping;

/// <summary>Müşteri entity ↔ DTO/command eşlemeleri. Her entity bu desende kendi dosyasını tutar.</summary>
public sealed class CustomerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerDto>();

        // Kimlik ve denetim alanları servis katmanında set edilir; eşlemede yok sayılır.
        config.NewConfig<CreateCustomerCommand, Customer>()
            .Ignore(nameof(Customer.Id),
                nameof(Customer.CreatedAtUtc),
                nameof(Customer.UpdatedAtUtc),
                nameof(Customer.CreatedBy),
                nameof(Customer.UpdatedBy));

        config.NewConfig<UpdateCustomerCommand, Customer>()
            .Ignore(nameof(Customer.Id),
                nameof(Customer.CreatedAtUtc),
                nameof(Customer.UpdatedAtUtc),
                nameof(Customer.CreatedBy),
                nameof(Customer.UpdatedBy));
    }
}
