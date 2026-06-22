namespace Turbohesap.Shared.Security;

/// <summary>
/// Sistemdeki tüm rol adları. Hem API yetkilendirmesi ([Authorize(Roles = ...)]) hem de
/// Blazor arayüzü bu sabitleri kullanır; rol adları tek yerde tutulur (req 11).
/// </summary>
public static class Roles
{
    /// <summary>Tam yetkili sistem yöneticisi.</summary>
    public const string Administrator = "Administrator";

    /// <summary>Yönetici; operasyonel modülleri yönetir.</summary>
    public const string Manager = "Manager";

    /// <summary>Muhasebeci; finansal kayıtlara erişir.</summary>
    public const string Accountant = "Accountant";

    /// <summary>Standart kullanıcı; sınırlı okuma yetkisi.</summary>
    public const string User = "User";

    /// <summary>Tanımlı tüm roller.</summary>
    public static readonly IReadOnlyList<string> All = [Administrator, Manager, Accountant, User];
}
