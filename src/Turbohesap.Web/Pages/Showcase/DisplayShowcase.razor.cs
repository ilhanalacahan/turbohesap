using Microsoft.AspNetCore.Components;
using System;

namespace Turbohesap.Web.Pages.Showcase;

/// <summary>
/// ThCard, ThBadge, ThAvatar ve ThProgress bileşenleri için test vitrini.
/// </summary>
public partial class DisplayShowcase : ComponentBase
{
    private double _progressValue = 45;

    private void IncreaseProgress()
    {
        _progressValue = Math.Min(100, _progressValue + 10);
    }

    private void DecreaseProgress()
    {
        _progressValue = Math.Max(0, _progressValue - 10);
    }
}
