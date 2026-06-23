using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Components.Layout;

/// <summary>
/// Sayfa sarmalayıcı. Render edildiğinde geçerli rotayı PageTabs'a kaydeder (sekme yoksa açar,
/// varsa odaklanır). Böylece sekmeler yalnızca gerçek sayfalar için oluşur; login/not-found/hata
/// gibi ThPage kullanmayan sayfalar sekme oluşturmaz. <see cref="RegisterTab"/>=false ile bir
/// sayfa sekme oluşturmamayı seçebilir.
/// </summary>
public partial class ThPage
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public bool Flush { get; set; }
    [Parameter] public bool FooterFixed { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Actions { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }

    /// <summary>Sekme başlığı (boşsa <see cref="Title"/> kullanılır).</summary>
    [Parameter] public string? TabTitle { get; set; }

    /// <summary>Sekme ikonu (Font Awesome sınıfı).</summary>
    [Parameter] public string TabIcon { get; set; } = "fa-solid fa-file";

    /// <summary>Sayfanın bir PageTab oluşturup oluşturmayacağı (varsayılan: evet).</summary>
    [Parameter] public bool RegisterTab { get; set; } = true;

    private PageTabHandle? _handle;

    protected override void OnInitialized()
    {
        if (!RegisterTab)
        {
            return;
        }

        var href = new Uri(Nav.Uri).AbsolutePath;
        Tabs.Register(href, TabTitle ?? Title ?? "Sayfa", TabIcon);
        _handle = new PageTabHandle(Tabs, href);
    }
}
