using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Turbohesap.Api.Entities;

namespace Turbohesap.Api.Persistence.Configurations;

/// <summary>
/// Denetim kaydı tablosu. Değişen alanlar jsonb olarak tutulur; sorgulamayı hızlandırmak
/// için entity türü, kimliği, işlem ve tarih üzerinde indeksler vardır (req 3).
/// </summary>
public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityByDefaultColumn();

        builder.Property(x => x.EntityType).HasMaxLength(128).IsRequired();
        builder.Property(x => x.EntityId).HasMaxLength(64);
        builder.Property(x => x.TableName).HasMaxLength(128);
        builder.Property(x => x.Action).HasMaxLength(16).IsRequired();
        builder.Property(x => x.Changes).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.UserId).HasMaxLength(64);
        builder.Property(x => x.UserName).HasMaxLength(256);

        builder.HasIndex(x => x.EntityType);
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.CreatedAtUtc);
        builder.HasIndex(x => new { x.EntityType, x.EntityId });
        // jsonb içinde alan araması için GIN indeksi.
        builder.HasIndex(x => x.Changes).HasMethod("gin");
    }
}
