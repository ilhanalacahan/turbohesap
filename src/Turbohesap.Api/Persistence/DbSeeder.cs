using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Common;
using Turbohesap.Api.Entities;
using Turbohesap.Shared.Security;

namespace Turbohesap.Api.Persistence;

/// <summary>
/// Başlangıçta bekleyen migration'ları uygular ve ilk çalıştırmada bir yönetici kullanıcısı
/// oluşturur. Üretimde tohumlama (seeding) ayrı yönetilmelidir.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            db.Users.Add(new User
            {
                Email = "admin@turbohesap.local",
                FullName = "Sistem Yöneticisi",
                PasswordHash = PasswordHasher.Hash("Admin123!"),
                IsActive = true,
                Roles = [Roles.Administrator],
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = "system"
            });

            await db.SaveChangesAsync();
        }
    }
}
