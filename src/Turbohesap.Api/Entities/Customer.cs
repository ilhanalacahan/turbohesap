namespace Turbohesap.Api.Entities;

/// <summary>Müşteri (cari) varlığı — örnek dikey kesit için referans entity.</summary>
public sealed class Customer : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? TaxNumber { get; set; }

    public bool IsActive { get; set; } = true;
}
