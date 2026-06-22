namespace Turbohesap.Web.Services;

/// <summary>Tarayıcıda (ProtectedLocalStorage) saklanan oturum bilgisi.</summary>
public sealed class UserSession
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc.AddSeconds(-30);
}

/// <summary>Geçerli devre (circuit) için oturumu bellekte tutar (scoped).</summary>
public sealed class TokenStore
{
    public UserSession? Session { get; set; }
}
