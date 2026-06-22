using System.Security.Claims;

namespace Turbohesap.Api.Common;

/// <summary>
/// İçinde bulunulan isteğin kullanıcı/istemci bağlamı. Denetim (audit), hata kaydı ve
/// varlıkların CreatedBy/UpdatedBy alanları bu bilgiyi kullanır.
/// </summary>
public interface ICurrentUser
{
    string? UserId { get; }
    string? UserName { get; }
    string? IpAddress { get; }
    bool IsAuthenticated { get; }
    IReadOnlyList<string> Roles { get; }
}

/// <summary><see cref="IHttpContextAccessor"/> üzerinden geçerli kullanıcıyı çözer.</summary>
public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => Principal?.FindFirstValue(ClaimTypes.Email) ?? Principal?.Identity?.Name;

    public string? IpAddress => accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public IReadOnlyList<string> Roles =>
        Principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? [];
}
