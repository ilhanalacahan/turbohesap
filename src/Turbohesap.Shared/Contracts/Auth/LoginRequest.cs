namespace Turbohesap.Shared.Contracts.Auth;

/// <summary>Giriş isteği gövdesi.</summary>
public sealed class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
