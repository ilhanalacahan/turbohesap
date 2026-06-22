using Audit.EntityFramework;

namespace Turbohesap.Api.Entities;

/// <summary>
/// EF Core seviyesinde yakalanan denetim kaydı (req 3). Audit.NET, denetlenen her
/// varlık değişikliği için bu tipten bir satır üretir ve aynı transaction içinde yazar.
/// <see cref="AuditIgnoreAttribute"/> ile kendisi denetlenmez (özyineleme önlenir).
/// </summary>
[AuditIgnore]
public sealed class AuditLog
{
    public long Id { get; set; }

    /// <summary>Değişen varlık tipinin adı (örn. "Customer").</summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>Değişen varlığın birincil anahtarı (string olarak).</summary>
    public string? EntityId { get; set; }

    /// <summary>Veritabanı tablo adı.</summary>
    public string? TableName { get; set; }

    /// <summary>İşlem türü: Insert / Update / Delete.</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Değişen alanlar ve eski/yeni değerleri (jsonb).</summary>
    public string Changes { get; set; } = "{}";

    public string? IpAddress { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
