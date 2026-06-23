using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Common;
using Turbohesap.Api.Entities;
using Turbohesap.Api.Persistence;
using Turbohesap.Shared.Contracts.Diagnostics;
using Turbohesap.Shared.Services;

namespace Turbohesap.Api.Services;

/// <summary>
/// Web (Blazor) tarafında yakalanan istemci hatalarını <c>error_logs</c> tablosuna yazar.
/// Sunucu tarafı <see cref="Middleware.GlobalExceptionMiddleware"/> ile aynı hash + tekrar
/// (OccurrenceCount) mantığını kullanır; bu kayıtlar <c>Source = "Web"</c> ile işaretlenir.
/// </summary>
public sealed class WebErrorLogService(AppDbContext db, ICurrentUser currentUser) : IWebErrorLogService
{
    private const string WebSource = "Web";

    public async Task<bool> LogAsync(LogWebErrorCommand command, CancellationToken cancellationToken = default)
    {
        var hash = ComputeHash(command);
        var now = DateTime.UtcNow;

        var existing = await db.ErrorLogs.FirstOrDefaultAsync(e => e.Hash == hash, cancellationToken);
        if (existing is not null)
        {
            existing.OccurrenceCount += 1;
            existing.LastSeenAtUtc = now;
        }
        else
        {
            db.ErrorLogs.Add(new ErrorLog
            {
                Hash = hash,
                Message = command.Message,
                ExceptionType = command.ExceptionType,
                StackTrace = command.StackTrace,
                Source = command.Source ?? WebSource,
                HttpMethod = WebSource,
                Path = command.Path,
                StatusCode = 0,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                UserId = currentUser.UserId,
                UserName = currentUser.UserName,
                OccurrenceCount = 1,
                FirstSeenAtUtc = now,
                LastSeenAtUtc = now
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string ComputeHash(LogWebErrorCommand command)
    {
        var signature = $"{WebSource}|{command.ExceptionType}|{command.Message}|{command.Path}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(signature));
        return Convert.ToHexStringLower(bytes);
    }
}
