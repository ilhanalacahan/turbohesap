using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Turbohesap.Api.Entities;

namespace Turbohesap.Api.Persistence.Configurations;

/// <summary>Müşteri tablosu yapılandırması. Tablo/kolon adları snake_case'e dönüştürülür.</summary>
public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(32).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.PhoneNumber).HasMaxLength(32);
        builder.Property(x => x.TaxNumber).HasMaxLength(32);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedBy).HasMaxLength(256);
        builder.Property(x => x.UpdatedBy).HasMaxLength(256);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.IsActive);
    }
}
