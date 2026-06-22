using Turbohesap.Shared.Cqrs;

namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>Yeni müşteri oluşturur. Oluşan kaydı DTO olarak döner.</summary>
public sealed class CreateCustomerCommand : ICommand<CustomerDto>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsActive { get; set; } = true;
}
