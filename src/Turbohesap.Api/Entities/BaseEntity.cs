namespace Turbohesap.Api.Entities;

/// <summary>
/// Tüm iş varlıklarının ortak temeli. Kimlik ve denetim alanlarını (kim/ne zaman)
/// taşır. Alan adları İngilizce, veritabanında snake_case'e çevrilir (req 1, snake_case).
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
