using Turbohesap.Web.Models;

namespace Turbohesap.Web.Services;

/// <summary>
/// PageTab yönetimi. Sekmeler sayfanın kendisi tarafından açılır (ThPage → <see cref="Register"/>),
/// bu yüzden bulunamayan/serbest sayfalar (login, not-found, hata) sekme oluşturmaz. Her sekme
/// kirli (kaydedilmemiş değişiklik) durumunu taşır; kirli sekme kapatılırken UI onay sorar.
/// </summary>
public sealed class PageTabService
{
    private readonly List<PageTab> _tabs = [];

    public IReadOnlyList<PageTab> Tabs => _tabs;
    public string? ActiveHref { get; private set; }

    public event Action? OnChange;

    /// <summary>Sayfa açıldığında çağrılır: sekme yoksa açar, varsa başlığı tazeler ve odaklanır.</summary>
    public void Register(string href, string title, string icon)
    {
        var existing = _tabs.FirstOrDefault(t => t.Href == href);
        if (existing is null)
        {
            _tabs.Add(new PageTab { Title = title, Href = href, Icon = icon });
        }
        else
        {
            existing.Title = title;
            existing.Icon = icon;
        }
        ActiveHref = href;
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

    /// <summary>Sekmenin kaydedilmemiş değişiklik durumunu günceller.</summary>
    public void SetDirty(string href, bool dirty)
    {
        var tab = _tabs.FirstOrDefault(t => t.Href == href);
        if (tab is not null && tab.IsDirty != dirty)
        {
            tab.IsDirty = dirty;
            OnChange?.Invoke();
        }
    }

    public bool IsDirty(string href) => _tabs.FirstOrDefault(t => t.Href == href)?.IsDirty ?? false;

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

/// <summary>
/// Bir sayfanın kendi sekmesine eriştiği tutamaç. ThPage tarafından cascading değer olarak
/// sağlanır; sayfalar formları değiştikçe <see cref="MarkDirty"/>/<see cref="MarkClean"/> çağırır.
/// </summary>
public sealed class PageTabHandle(PageTabService service, string href)
{
    public string Href { get; } = href;

    public void SetDirty(bool dirty) => service.SetDirty(Href, dirty);
    public void MarkDirty() => SetDirty(true);
    public void MarkClean() => SetDirty(false);
}
