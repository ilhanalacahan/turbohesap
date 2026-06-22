using Microsoft.EntityFrameworkCore;
using Turbohesap.Api.Authentication;
using Turbohesap.Api.Common;
using Turbohesap.Api.Persistence;
using Turbohesap.Shared.Contracts.Auth;
using Turbohesap.Shared.Services;

namespace Turbohesap.Api.Services;

/// <summary>Kimlik doğrulama iş mantığı. Parola PBKDF2 ile doğrulanır, JWT üretilir.</summary>
public sealed class AuthService(AppDbContext db, JwtTokenService tokenService) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive, cancellationToken);

        if (user is null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AuthenticationFailedException("E-posta veya parola hatalı.");
        }

        var (accessToken, expiresAt) = tokenService.CreateToken(user);

        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAt,
            Email = user.Email,
            FullName = user.FullName,
            Roles = user.Roles
        };
    }
}
