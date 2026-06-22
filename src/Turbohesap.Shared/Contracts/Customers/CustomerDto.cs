namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>Müşteri okuma modeli. API ve Blazor arasında taşınır.</summary>
public sealed class CustomerDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
