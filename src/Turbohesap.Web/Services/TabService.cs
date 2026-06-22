using Turbohesap.Web.Models;

namespace Turbohesap.Web.Services;

/// <summary>
/// Sekme yönetimi (req 29). Menüden bir öğeye tıklanınca sayfa zaten açıksa o sekmeye
/// odaklanılır, değilse yeni sekme açılır. Sekmesiz bağımsız sayfalar da olabilir.
/// </summary>
public sealed class TabService
{
    private readonly List<TabItem> _tabs = [];

    public IReadOnlyList<TabItem> Tabs => _tabs;
    public string? ActiveHref { get; private set; }

    public event Action? OnChange;

    /// <summary>Sekme açıksa odaklanır, değilse açar. Açık sekmenin href'ini döner.</summary>
    public void OpenOrFocus(TabItem tab)
    {
        if (!_tabs.Any(t => t.Href == tab.Href))
        {
            _tabs.Add(tab);
        }
        ActiveHref = tab.Href;
        OnChange?.Invoke();
    }

    public void SetActive(string href)
    {
        if (_tabs.Any(t => t.Href == href))
        {
            ActiveHref = href;
            OnChange?.Invoke();
        }
    }

    /// <summary>Sekmeyi kapatır ve odaklanılacak komşu sekmenin href'ini döner (yoksa null).</summary>
    public string? Close(string href)
    {
        var index = _tabs.FindIndex(t => t.Href == href);
        if (index < 0)
        {
            return ActiveHref;
        }

        _tabs.RemoveAt(index);

        if (ActiveHref == href)
        {
            var neighbour = _tabs.ElementAtOrDefault(Math.Max(0, index - 1));
            ActiveHref = neighbour?.Href;
        }

        OnChange?.Invoke();
        return ActiveHref;
    }
}
