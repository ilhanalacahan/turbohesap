namespace Turbohesap.Web.Models;

/// <summary>Açık bir sekme (req 29: menüden açılan sayfalar sekme olarak yönetilir).</summary>
public sealed class TabItem
{
    public required string Title { get; init; }
    public required string Href { get; init; }
    public string Icon { get; init; } = "fa-solid fa-file";
    public bool Closable { get; init; } = true;
}
