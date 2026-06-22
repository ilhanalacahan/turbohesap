namespace Turbohesap.Shared.Security;

/// <summary>
/// API sürüm sabitleri. Tüm controller'lar bu sabitleri kullanarak standardize edilir (req 12).
/// Rota şablonu: <c>api/v{version}/...</c>
/// </summary>
public static class ApiVersions
{
    /// <summary>v1 — anlamsal sürüm dizesi (Asp.Versioning için).</summary>
    public const string V1 = "1.0";

    /// <summary>Tüm controller'ların paylaştığı temel rota öneki.</summary>
    public const string RoutePrefix = "api/v{version:apiVersion}";
}
