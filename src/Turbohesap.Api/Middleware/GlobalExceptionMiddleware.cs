using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Common;
using Turbohesap.Api.Entities;
using Turbohesap.Api.Persistence;
using Turbohesap.Shared.Common;

namespace Turbohesap.Api.Middleware;

/// <summary>
/// Küresel hata yakalama middleware'i (req 2). Bilinen alan istisnalarını uygun HTTP
/// durum koduna; beklenmeyen (500) hataları ise error_logs tablosuna tüm bağlam bilgisiyle
/// yazar. Her yanıt daima <c>{"detail":"mesaj"}</c> biçimindedir.
/// </summary>
public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IServiceScopeFactory scopeFactory)
{
    private const string GenericMessage =
        "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.";

    private static readonly string[] SensitiveHeaders = ["Authorization", "Cookie", "Set-Cookie"];

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, detail, isUnexpected) = Map(exception);

        if (isUnexpected)
        {
            logger.LogError(exception, "İşlenmeyen hata: {Message}", exception.Message);
            await PersistErrorAsync(context, exception, statusCode);
        }
        else
        {
            logger.LogWarning("İşlenen alan hatası ({Status}): {Message}", statusCode, exception.Message);
        }

        if (context.Response.HasStarted)
        {
            return; // Yanıt başladıysa gövdeyi değiştiremeyiz.
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new ProblemDetail(detail));
    }

    private static (int StatusCode, string Detail, bool IsUnexpected) Map(Exception exception) => exception switch
    {
        ValidationException ve => (StatusCodes.Status400BadRequest,
            string.Join(" ", ve.Errors.Select(e => e.ErrorMessage).Distinct()), false),
        BusinessRuleException => (StatusCodes.Status400BadRequest, exception.Message, false),
        AuthenticationFailedException => (StatusCodes.Status401Unauthorized, exception.Message, false),
        NotFoundException => (StatusCodes.Status404NotFound, exception.Message, false),
        ConflictException => (StatusCodes.Status409Conflict, exception.Message, false),
        _ => (StatusCodes.Status500InternalServerError, GenericMessage, true)
    };

    private async Task PersistErrorAsync(HttpContext context, Exception exception, int statusCode)
    {
        try
        {
            var (fileName, lineNumber) = ResolveSource(exception);
            var hash = ComputeHash(exception, fileName, lineNumber);
            var now = DateTime.UtcNow;

            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUser>();

            var existing = await db.ErrorLogs.FirstOrDefaultAsync(e => e.Hash == hash);
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
                    Message = exception.Message,
                    ExceptionType = exception.GetType().FullName ?? exception.GetType().Name,
                    StackTrace = exception.StackTrace,
                    Source = exception.Source,
                    FileName = fileName,
                    LineNumber = lineNumber,
                    HttpMethod = context.Request.Method,
                    Path = context.Request.Path.Value,
                    QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,
                    StatusCode = statusCode,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = context.Request.Headers.UserAgent.ToString(),
                    Headers = SerializeHeaders(context),
                    UserId = currentUser.UserId,
                    UserName = currentUser.UserName,
                    OccurrenceCount = 1,
                    FirstSeenAtUtc = now,
                    LastSeenAtUtc = now
                });
            }

            await db.SaveChangesAsync();
        }
        catch (Exception logEx)
        {
            // Hata kaydı yazılamazsa isteği düşürmüyoruz; yalnızca logluyoruz.
            logger.LogError(logEx, "error_logs tablosuna yazılamadı.");
        }
    }

    private static (string? FileName, int? LineNumber) ResolveSource(Exception exception)
    {
        var trace = new StackTrace(exception, fNeedFileInfo: true);
        foreach (var frame in trace.GetFrames())
        {
            var file = frame.GetFileName();
            if (!string.IsNullOrEmpty(file))
            {
                return (Path.GetFileName(file), frame.GetFileLineNumber());
            }
        }

        return (null, null);
    }

    private static string ComputeHash(Exception exception, string? fileName, int? lineNumber)
    {
        var signature = $"{exception.GetType().FullName}|{exception.Message}|{fileName}|{lineNumber}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(signature));
        return Convert.ToHexStringLower(bytes);
    }

    private static string SerializeHeaders(HttpContext context)
    {
        var headers = context.Request.Headers
            .Where(h => !SensitiveHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase))
            .ToDictionary(h => h.Key, h => h.Value.ToString());
        return JsonSerializer.Serialize(headers);
    }
}
