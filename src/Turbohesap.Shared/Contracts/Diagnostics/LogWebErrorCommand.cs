using Turbohesap.Shared.Cqrs;

namespace Turbohesap.Shared.Contracts.Diagnostics;

/// <summary>
/// Web istemci hatasını kalıcılaştırma komutu. Controller, <see cref="WebErrorReport"/> gövdesini
/// ve sunucu bağlamını (IP, user-agent) bu komuta doldurup veriyoluna gönderir; handler servise
/// delege eder. Sonuç: kayıt yazıldı/güncellendi mi.
/// </summary>
public sealed class LogWebErrorCommand : ICommand<bool>
{
    public string Message { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? Source { get; set; }
    public string? Path { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
