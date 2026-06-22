using Turbohesap.Shared.Contracts.Auth;

namespace Turbohesap.Web.Services;

/// <summary>İstemci tarafı kimlik doğrulama akışı: API'ye giriş, oturum saklama, çıkış.</summary>
public sealed class AuthClientService(ApiClient api, TurbohesapAuthStateProvider authState)
{
    public async Task LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await api.PostAsync<LoginResponse>("api/v1/auth/login", request, cancellationToken)
            ?? throw new ApiException(500, "Sunucudan geçersiz yanıt alındı.");

        await authState.SignInAsync(new UserSession
        {
            AccessToken = response.AccessToken,
            ExpiresAtUtc = response.ExpiresAtUtc,
            Email = response.Email,
            FullName = response.FullName,
            Roles = [.. response.Roles]
        });
    }

    public Task LogoutAsync() => authState.SignOutAsync();
}
