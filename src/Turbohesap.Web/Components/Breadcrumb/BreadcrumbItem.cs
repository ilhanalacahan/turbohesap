namespace Turbohesap.Web.Components.Breadcrumb;

/// <summary>
/// Ekmek kırıntısı (breadcrumb) öğe modeli.
/// </summary>
public class BreadcrumbItem
{
    public string Text { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Icon { get; set; }
}
