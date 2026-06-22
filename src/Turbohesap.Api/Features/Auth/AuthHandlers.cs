using Turbohesap.Shared.Contracts.Auth;
using Turbohesap.Shared.Services;
using Wolverine.Attributes;

namespace Turbohesap.Api.Features.Auth;

/// <summary>
/// Kimlik doğrulama handler'ı. Giriş isteği de Wolverine üzerinden geçer; böylece
/// FluentValidation middleware'i <see cref="LoginRequest"/>'i otomatik doğrular (req 5, 6).
/// </summary>
[WolverineHandler]
public sealed class AuthHandlers
{
    public static Task<LoginResponse> Handle(
        LoginRequest request, IAuthService service, CancellationToken cancellationToken)
        => service.LoginAsync(request, cancellationToken);
}
