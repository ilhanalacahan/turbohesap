namespace Turbohesap.Api.Authentication;

/// <summary>JWT üretim/doğrulama ayarları. <c>appsettings.json</c> içindeki "Jwt" bölümünden bağlanır.</summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "Turbohesap";
    public string Audience { get; set; } = "Turbohesap";

    /// <summary>HMAC-SHA256 imzalama anahtarı. Üretimde gizli (secret) olarak saklanmalıdır.</summary>
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>Erişim token'ı geçerlilik süresi (dakika).</summary>
    public int AccessTokenMinutes { get; set; } = 120;
}
