using Microsoft.AspNetCore.Components;
using System;

namespace Turbohesap.Web.Components.Feedback;

/// <summary>
/// ThModalHost code-behind class.
/// ModalService olaylarını dinleyerek aktif modal listesini günceller.
/// </summary>
public partial class ThModalHost : IDisposable
{
    protected override void OnInitialized()
    {
        ModalService.OnChange += StateChanged;
    }

    private void StateChanged() => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        ModalService.OnChange -= StateChanged;
    }
}
