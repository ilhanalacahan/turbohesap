using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Turbohesap.Api.Entities;

namespace Turbohesap.Api.Persistence.Configurations;

/// <summary>Hata kaydı tablosu. Hash üzerinde indeks ile tekrar tespiti hızlandırılır (req 2).</summary>
public sealed class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.ToTable("error_logs");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityByDefaultColumn();

        builder.Property(x => x.Hash).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.ExceptionType).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Source).HasMaxLength(256);
        builder.Property(x => x.FileName).HasMaxLength(512);
        builder.Property(x => x.HttpMethod).HasMaxLength(16);
        builder.Property(x => x.Path).HasMaxLength(2048);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasMaxLength(512);
        builder.Property(x => x.Headers).HasColumnType("jsonb");
        builder.Property(x => x.UserId).HasMaxLength(64);
        builder.Property(x => x.UserName).HasMaxLength(256);

        builder.HasIndex(x => x.Hash);
        builder.HasIndex(x => x.LastSeenAtUtc);
        builder.HasIndex(x => x.StatusCode);
    }
}
