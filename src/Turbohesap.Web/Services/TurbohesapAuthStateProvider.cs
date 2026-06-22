using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Turbohesap.Web.Services;

/// <summary>
/// JWT tabanlı kimlik durumu sağlayıcısı. Oturum ProtectedLocalStorage'da saklanır,
/// devre içinde <see cref="TokenStore"/>'da önbelleklenir. Roller claim'lere dönüştürülür;
/// böylece [Authorize(Roles=...)] ve AuthorizeView Blazor'da çalışır (req 28).
/// </summary>
public sealed class TurbohesapAuthStateProvider(ProtectedLocalStorage storage, TokenStore tokenStore)
    : AuthenticationStateProvider
{
    private const string StorageKey = "th.session";
    private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public UserSession? CurrentSession => tokenStore.Session;

    public async Task SignInAsync(UserSession session)
    {
        tokenStore.Session = session;
        await storage.SetAsync(StorageKey, session);
        NotifyAuthenticationStateChanged(Task.FromResult(BuildState(session)));
    }

    public async Task SignOutAsync()
    {
        tokenStore.Session = null;
        await storage.DeleteAsync(StorageKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (tokenStore.Session is null)
        {
            try
            {
                var result = await storage.GetAsync<UserSession>(StorageKey);
                if (result.Success && result.Value is not null)
                {
                    tokenStore.Session = result.Value;
                }
            }
            catch
            {
                // Prerender sırasında JS interop kullanılamaz; anonim dön.
                return Anonymous;
            }
        }

        var session = tokenStore.Session;
        if (session is null || session.IsExpired)
        {
            return Anonymous;
        }

        return BuildState(session);
    }

    private static AuthenticationState BuildState(UserSession session)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, session.FullName),
            new(ClaimTypes.Email, session.Email)
        };
        claims.AddRange(session.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, authenticationType: "jwt", ClaimTypes.Name, ClaimTypes.Role);
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}
