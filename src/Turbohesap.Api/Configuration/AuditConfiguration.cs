using System.Text.Json;
using Audit.Core;
using Audit.EntityFramework;
using Turbohesap.Api.Entities;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// Audit.NET'i EF Core seviyesinde yapılandırır (req 3). Denetlenen her varlık değişikliği,
/// aynı DbContext/transaction içinde bir <see cref="AuditLog"/> satırına yazılır. Değişen
/// alanlar jsonb olarak saklanır; IP/kullanıcı bilgisi HTTP bağlamından zenginleştirilir.
/// </summary>
public static class AuditConfiguration
{
    /// <summary>Denetim olaylarını zenginleştirmek için geçerli isteğin HTTP bağlamı.</summary>
    internal static IHttpContextAccessor? HttpContextAccessor { get; private set; }

    public static WebApplicationBuilder AddAuditing(this WebApplicationBuilder builder)
    {
        Audit.Core.Configuration.Setup()
            .UseEntityFramework(ef => ef
                .AuditTypeMapper(_ => typeof(AuditLog))
                .AuditEntityAction<AuditLog>((auditEvent, entry, auditLog) =>
                {
                    var httpContext = HttpContextAccessor?.HttpContext;

                    auditLog.EntityType = entry.EntityType?.Name ?? entry.Table;
                    auditLog.TableName = entry.Table;
                    auditLog.EntityId = entry.PrimaryKey.Count > 0
                        ? string.Join(",", entry.PrimaryKey.Values)
                        : null;
                    auditLog.Action = entry.Action;
                    auditLog.Changes = JsonSerializer.Serialize(new
                    {
                        entry.Changes,
                        entry.ColumnValues
                    });
                    auditLog.IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
                    auditLog.UserId = httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    auditLog.UserName = httpContext?.User.Identity?.Name
                        ?? httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                    auditLog.CreatedAtUtc = DateTime.UtcNow;
                })
                .IgnoreMatchedProperties());

        return builder;
    }

    /// <summary>Denetim zenginleştirmesi için HTTP bağlam erişimcisini bağlar.</summary>
    public static WebApplication UseAuditing(this WebApplication app)
    {
        HttpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        return app;
    }
}
