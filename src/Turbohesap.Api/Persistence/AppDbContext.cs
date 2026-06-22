using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Entities;

namespace Turbohesap.Api.Persistence;

/// <summary>
/// Uygulama veritabanı bağlamı. <see cref="AuditDbContext"/>'ten türer; böylece tüm
/// değişiklikler Audit.NET tarafından EF seviyesinde yakalanır (req 3). Entity
/// konfigürasyonları ayrı dosyalardadır ve assembly'den otomatik register edilir (req 4).
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : AuditDbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Her entity kendi IEntityTypeConfiguration dosyasında tanımlıdır (req 4).
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
