namespace Turbohesap.Shared.Common;

/// <summary>
/// API'nin döndürdüğü standart hata gövdesi. Küresel hata yakalama middleware'i
/// ve doğrulama hataları her zaman bu şekli üretir: <c>{"detail":"mesaj"}</c> (req 2).
/// </summary>
public sealed class ProblemDetail
{
    public ProblemDetail() { }

    public ProblemDetail(string detail) => Detail = detail;

    /// <summary>Kullanıcıya gösterilebilir hata mesajı.</summary>
    public string Detail { get; set; } = string.Empty;
}
