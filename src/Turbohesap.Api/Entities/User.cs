namespace Turbohesap.Api.Entities;

/// <summary>Uygulama kullanıcısı. Kimlik doğrulama ve rol bilgisini taşır.</summary>
public sealed class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    /// <summary>PBKDF2 ile türetilmiş parola özeti (salt dahil).</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    /// <summary>Kullanıcının rolleri. PostgreSQL'de <c>text[]</c> kolonuna yazılır.</summary>
    public List<string> Roles { get; set; } = [];
}
