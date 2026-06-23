namespace Turbohesap.Web.Models;

/// <summary>
/// Bir PageTab — ana içerik alanında açık olan sayfa sekmesi. Normal (UI) sekmelerden
/// farklıdır: yalnızca gerçek sayfalar tarafından (ThPage aracılığıyla) açılır ve
/// kaydedilmemiş değişiklik (<see cref="IsDirty"/>) durumunu taşır. Kirli bir sekme
/// kapatılırken kullanıcıya onay sorulur.
/// </summary>
public sealed class PageTab
{
    public required string Title { get; set; }
    public required string Href { get; init; }
    public string Icon { get; set; } = "fa-solid fa-file";
    public bool Closable { get; init; } = true;

    /// <summary>Sayfada kaydedilmemiş değişiklik var mı? Sekme şeridinde nokta ile gösterilir.</summary>
    public bool IsDirty { get; set; }
}
