namespace Turbohesap.Shared.Contracts.Auth;

/// <summary>Başarılı giriş yanıtı: JWT erişim token'ı ve kullanıcı özeti.</summary>
public sealed class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public IReadOnlyList<string> Roles { get; set; } = [];
}
