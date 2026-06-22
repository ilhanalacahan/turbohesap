using Turbohesap.Shared.Cqrs;

namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>Var olan müşteriyi günceller.</summary>
public sealed class UpdateCustomerCommand : ICommand<CustomerDto>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsActive { get; set; } = true;
}
