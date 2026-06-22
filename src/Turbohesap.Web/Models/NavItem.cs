namespace Turbohesap.Web.Models;

/// <summary>Kenar çubuğu menü öğesi. Ağaç yapısı için <see cref="Children"/> taşır (req 29).</summary>
public sealed class NavItem
{
    public required string Label { get; init; }

    /// <summary>Font Awesome ikon sınıfı (örn. "fa-solid fa-users").</summary>
    public string Icon { get; init; } = "fa-solid fa-circle";

    /// <summary>Gidilecek yol (yaprak öğeler için). Grup başlıkları için null.</summary>
    public string? Href { get; init; }

    /// <summary>Yalnızca bu rollere sahip kullanıcılar görür (boşsa herkes).</summary>
    public IReadOnlyList<string> Roles { get; init; } = [];

    public IReadOnlyList<NavItem> Children { get; init; } = [];

    public bool HasChildren => Children.Count > 0;
}

/// <summary>Adlandırılmış menü grubu (sidebar'da başlık altında gruplanır).</summary>
public sealed class NavGroup
{
    public required string Title { get; init; }
    public IReadOnlyList<NavItem> Items { get; init; } = [];
}
