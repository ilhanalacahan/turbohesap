namespace Turbohesap.Web.Models;

/// <summary>Uygulama başlatıcıdaki bir uygulama (Microsoft 365 tarzı ızgara, req 29).</summary>
public sealed class AppItem
{
    public required string Name { get; init; }
    public string Icon { get; init; } = "fa-solid fa-cube";
    public string Href { get; init; } = "#";
    public string? Description { get; init; }

    /// <summary>Kutucuk ikonu için vurgu rengi (CSS rengi). Boşsa birincil renk kullanılır.</summary>
    public string Accent { get; init; } = "var(--th-primary)";

    /// <summary>Henüz aktif değil ("Yakında" rozeti gösterilir).</summary>
    public bool ComingSoon => Href == "#";
}
