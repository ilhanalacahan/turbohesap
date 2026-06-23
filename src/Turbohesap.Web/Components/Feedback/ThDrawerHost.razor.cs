using Microsoft.AspNetCore.Components;
using System;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>
/// ThDrawerHost code-behind sınıfı.
/// DrawerService olaylarını dinleyerek aktif yan panel listesini günceller.
/// </summary>
public partial class ThDrawerHost : IDisposable
{
    protected override void OnInitialized()
    {
        DrawerService.OnChange += StateChanged;
    }

    private void StateChanged() => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        DrawerService.OnChange -= StateChanged;
    }
}
