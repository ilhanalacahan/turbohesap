using Turbohesap.Shared.Contracts.Auth;

namespace Turbohesap.Shared.Services;

/// <summary>
/// Kimlik doğrulama iş mantığı sözleşmesi. Uygulaması API tarafındadır ve
/// transactional çalışır (req 10). Arayüz Shared içinde tutulur (req 1).
/// </summary>
public interface IAuthService
{
    /// <summary>E-posta/parola doğrular ve JWT üretir. Hatalı kimlikte istisna fırlatır.</summary>
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
