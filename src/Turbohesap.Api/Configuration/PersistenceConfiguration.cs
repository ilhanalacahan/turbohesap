using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Persistence;

namespace Turbohesap.Api.Configuration;

/// <summary>PostgreSQL + EF Core kayıt noktası. snake_case adlandırma kuralı uygulanır.</summary>
public static class PersistenceConfiguration
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("'Default' bağlantı dizesi tanımlı değil.");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            // Tablo ve kolon adları snake_case'e çevrilir (kullanıcı talebi).
            options.UseSnakeCaseNamingConvention();
        });

        return builder;
    }
}
