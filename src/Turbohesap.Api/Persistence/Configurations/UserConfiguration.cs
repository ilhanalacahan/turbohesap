using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Turbohesap.Api.Entities;

namespace Turbohesap.Api.Persistence.Configurations;

/// <summary>Kullanıcı tablosu yapılandırması.</summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        // List<string> -> PostgreSQL text[]
        builder.Property(x => x.Roles).HasColumnType("text[]");

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
