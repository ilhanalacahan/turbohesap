namespace Turbohesap.Web.Models;

/// <summary>Uygulama başlatıcıdaki bir uygulama (Microsoft 365 tarzı ızgara, req 29).</summary>
public sealed class AppItem
{
    public required string Name { get; init; }
    public string Icon { get; init; } = "fa-solid fa-cube";
    public string Href { get; init; } = "#";
    public string? Description { get; init; }
}
