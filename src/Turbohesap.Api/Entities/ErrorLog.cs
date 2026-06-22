using Audit.EntityFramework;

namespace Turbohesap.Api.Entities;

/// <summary>
/// Küresel hata yakalama middleware'inin yazdığı 500 hata kaydı (req 2). Aynı hatanın
/// tekrarı, hash üzerinden tek satırda <see cref="OccurrenceCount"/> artırılarak izlenir.
/// </summary>
[AuditIgnore]
public sealed class ErrorLog
{
    public long Id { get; set; }

    /// <summary>Hatanın parmak izi (tip + mesaj + ilk stack çerçevesi). Tekrar tespiti için.</summary>
    public string Hash { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string ExceptionType { get; set; } = string.Empty;

    public string? StackTrace { get; set; }

    public string? Source { get; set; }

    /// <summary>Hatanın oluştuğu dosya adı.</summary>
    public string? FileName { get; set; }

    /// <summary>Hatanın oluştuğu satır numarası.</summary>
    public int? LineNumber { get; set; }

    public string? HttpMethod { get; set; }

    /// <summary>İstek yolu (URL path).</summary>
    public string? Path { get; set; }

    public string? QueryString { get; set; }

    public int StatusCode { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    /// <summary>İstek başlıkları (jsonb).</summary>
    public string? Headers { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    /// <summary>Bu hash'in toplam görülme sayısı (tekrar sayısı).</summary>
    public int OccurrenceCount { get; set; } = 1;

    public DateTime FirstSeenAtUtc { get; set; }

    public DateTime LastSeenAtUtc { get; set; }
}
