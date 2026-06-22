namespace Turbohesap.Shared.Contracts.Diagnostics;

/// <summary>
/// Web (Blazor) tarafında yakalanan bir istemci hatasının API'ye gönderilen gövdesi.
/// API bunu <c>error_logs</c> tablosuna (Source = "Web") işler; böylece web hataları da
/// sunucu hataları gibi merkezî olarak izlenir.
/// </summary>
public sealed class WebErrorReport
{
    public string Message { get; set; } = string.Empty;

    public string ExceptionType { get; set; } = string.Empty;

    public string? StackTrace { get; set; }

    public string? Source { get; set; }

    /// <summary>Hatanın oluştuğu istemci yolu (URL path).</summary>
    public string? Path { get; set; }
}
